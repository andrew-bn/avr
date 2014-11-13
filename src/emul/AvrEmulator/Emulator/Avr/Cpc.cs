using System;
using System.Runtime.ExceptionServices;

namespace Emulator.Avr
{
	public class Cpc: Instruction
	{
		public Cpc() : base("0000 01rd dddd rrrr")
		{
		}

		public override void Process(ExecutionState state)
		{
			var carry = state.Proc.StatusGet(Status.C) ? 1 : 0;
			var d = state.Proc.RegisterGet((Register)state.D);
			var r = state.Proc.RegisterGet((Register)state.R);

			var res = (byte)(d - r - carry);
			state.Proc.Status(Status.C, Math.Abs(d)<Math.Abs(r+carry));
			if (res!=0)
				state.Proc.StatusClear(Status.Z);

			
			state.Proc.Status(Status.N,( res & 0x80)!=0);

			state.Proc.StatusClear(Status.V);
			state.Proc.StatusClear(Status.H);
			state.Proc.Status(Status.S, state.Proc.StatusGet(Status.N)||state.Proc.StatusGet(Status.V));

			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}