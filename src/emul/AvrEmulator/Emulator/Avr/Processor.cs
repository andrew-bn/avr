using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.Avr
{
	public enum Port
	{
		SPL = 0x5d-0x20,
		SPH = 0x5E-0x20
	}

	public enum Register
	{
		R0=0,
		R1,R2,R3,R4,R5,R6,R7,R8,R9,R10,R11,R12,R13,R14,R15,R16,R17,R18,R19,R20,R21,R22,R23,R24,R25,R26,R27,R28,R29,R30,R31
	}
	public enum Status
	{
		C = 0,
		Z,
		N,
		V,
		S,
		H,
		T,
		I
	}
	public class Processor
	{
		public UInt16[] Flash;
		public byte[] Ram;
		public int PC;
		private static Instruction[] _instructions;

		static Processor()
		{
			_instructions = Assembly.GetExecutingAssembly().GetTypes()
				.Where(t => t.BaseType == typeof (Instruction))
				.Select(c=>Activator.CreateInstance(c))
				.Cast<Instruction>()
				.ToArray();

		}
		public bool StatusGet(Status st)
		{
			return (Ram[0x5f] & (1 << (int) st)) != 0;
		}
		public void StatusSet(Status st)
		{
			Ram[0x5f] = (byte)(Ram[0x5f]|(byte)(1<<(int)st));
		}
		public void StatusClear(Status st)
		{
			Ram[0x5f] = (byte)(Ram[0x5f] ^ (byte)(1 << (int)st));
		}
		public void PortSet(Port port, byte data)
		{
			Ram[(int)port + 0x20] = data;
		}

		public int SP
		{
			get { return PortGet(Port.SPL) | (PortGet(Port.SPH) << 8); }
			set { PortSet(Port.SPL, (byte)(value & 0xFF)); PortSet(Port.SPH, (byte)((value & 0xFF00)>>8)); }
		}
		public byte PortGet(Port port)
		{
			return PortGet((int)port);
		}
		public byte PortGet(int port)
		{
			return Ram[port + 0x20];
		}
		public void PortSet(int port, int register)
		{
			Ram[port + 0x20] = Ram[register];
		}
		public void RegisterSet(Register reg, byte data)
		{
			Ram[(int)reg] = data;
		}
		public byte RegisterGet(Register register)
		{
			return Ram[(int)register];
		}
		public void MemorySet(int address, byte data)
		{
			Ram[address] = data;
		}
		public UInt16 Instruction
		{
			get { return Flash[PC]; }
		}
		public UInt32 LInstruction
		{
			get { return (UInt32)(Flash[PC] << 16)|(Flash[PC+1]); }
		}
		public long Ticks;
		public Processor(UInt16[] flash)
		{
			PC = 0;
			Flash = flash;
			Ram = new byte[2 * 1024+0x60];
		}
		public void Run()
		{
			while (true)
				Step();
		}
		public void Step()
		{
			for (int i = 0;i<_instructions.Length;i++)
			{
				if (_instructions[i].Run(this)) 
					break;
			}
		}
		
		public void Tick(int cycles = 1)
		{
			Ticks += cycles;
		}
	}
}
