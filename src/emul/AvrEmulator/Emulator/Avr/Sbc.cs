using System;

namespace Emulator.Avr
{
	public class Sbc: Instruction
	{
		public Sbc()
			: base("0000 10rd dddd rrrr")
		{
			
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register) state.D);
			var r = state.Proc.RegisterGet((Register) state.R);
			var carry = (byte)( state.Proc.StatusGet(Status.C) ? 1 : 0);
			state.Proc.Status(Status.C, Math.Abs(v) < Math.Abs(r + carry));

			v -= r ;
			v -= carry;

			state.Proc.RegisterSet((Register)state.D, v);

			state.Proc.Status(Status.Z, v==0);
			state.Proc.Status(Status.N, (v & 0x80)!=0);

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