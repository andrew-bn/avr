using System;

namespace Emulator.Avr
{
	public class Sbci: Instruction
	{
		public Sbci()
			: base("0100 KKKK dddd KKKK")
		{
			
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register) state.D + 0x10);

			var carry = (byte)( state.Proc.StatusGet(Status.C) ? 1 : 0);
			state.Proc.Status(Status.C, Math.Abs(v) < Math.Abs(state.K + carry));

			v -= (byte)state.K ;
			v -= carry;

			state.Proc.RegisterSet((Register)state.D + 0x10, v);

			state.Proc.Status(Status.Z, v==0);
			state.Proc.Status(Status.N, (v & 0x80)==0x80);

			// dummy
			state.Proc.Status(Status.V,false);
			state.Proc.Status(Status.H, false);
			// end dummy

			state.Proc.Status(Status.S, state.Proc.StatusGet(Status.N) || state.Proc.StatusGet(Status.V));

			state.Proc.PC++;

			state.Proc.Tick();

		}
	}
}