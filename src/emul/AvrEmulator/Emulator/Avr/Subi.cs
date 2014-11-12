using System;

namespace Emulator.Avr
{
	public class Subi: Instruction
	{
		public Subi() : base("0101 KKKK dddd KKKK")
		{
			
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register) state.D + 0x10);

			state.Proc.Status(Status.C, Math.Abs(v) < Math.Abs(state.K));

			v -= (byte)state.K;

			state.Proc.RegisterSet((Register)state.D + 0x10, v);

			state.Proc.Status(Status.Z, v==0);
			state.Proc.Status(Status.N, (v & 0x80)==0x80);
			
			state.Proc.Status(Status.S, state.Proc.StatusGet(Status.N));

			state.Proc.PC++;

			state.Proc.Tick();

		}
	}
}