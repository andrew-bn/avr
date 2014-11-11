using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.Avr
{
	public class Processor
	{
		public UInt16[] Flash;
		public byte[] Ram;
		public int PC;
		private static Instruction[] _instructions;
		public List<int> AffectedAddresses { get; private set; }
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
			MemorySet(0x5f, (byte)(Ram[0x5f] | (byte)(1 << (int)st)));
		}
		public void StatusClear(Status st)
		{
			MemorySet(0x5f, (byte)(Ram[0x5f] ^ (byte)(1 << (int)st)));
		}
		public void PortSet(Port port, byte data)
		{
			MemorySet((int)port + 0x20, data);
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
		public void PortSet(int port, byte data)
		{
			MemorySet(port + 0x20, data);
		}
		public void RegisterSet(Register reg, byte data)
		{
			MemorySet((int)reg, data);
		}
		public byte RegisterGet(Register register)
		{
			return Ram[(int)register];
		}
		public void MemorySet(int address, byte data)
		{
			Ram[address] = data;
			if (!AffectedAddresses.Contains(address))
				AffectedAddresses.Add(address);
		}


		public long Ticks;
		public Processor(UInt16[] flash)
		{
			AffectedAddresses = new List<int>();
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
			AffectedAddresses.Clear();
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
