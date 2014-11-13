using System;

namespace Emulator.Avr
{
	public class Cp: Instruction
	{
		public Cp() : base("0001 01rd dddd rrrr")
		{
		}

		public override void Process(ExecutionState state)
		{
			var d = state.Proc.RegisterGet((Register)state.D);
			var r = state.Proc.RegisterGet((Register)state.R);


			state.Proc.Status(Status.C, Math.Abs(d)<Math.Abs(r));
			state.Proc.Status(Status.Z, d == r);

			var res =(byte)(d - r);
			state.Proc.Status(Status.N,( res & 0x80)!=0);

			state.Proc.StatusClear(Status.V);
			state.Proc.StatusClear(Status.H);
			state.Proc.Status(Status.S, state.Proc.StatusGet(Status.N)||state.Proc.StatusGet(Status.V));

			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}