namespace Emulator.Avr
{
	public class Dec: Instruction
	{
		public Dec() : base("1001 010d dddd 1010")
		{
		}
		
		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)state.D);

			if (v == 0x80) state.Proc.StatusSet(Status.V);
			else state.Proc.StatusClear(Status.V);

			v--;

			if (v == 0) state.Proc.StatusSet(Status.Z);
			else state.Proc.StatusClear(Status.Z);

			if ((v & 0x80) == 0x80) state.Proc.StatusSet(Status.N);
			else state.Proc.StatusClear(Status.N);

			if (state.Proc.StatusGet(Status.V) || state.Proc.StatusGet(Status.N))
				state.Proc.StatusSet(Status.S);
			else state.Proc.StatusClear(Status.S);

			state.Proc.RegisterSet((Register)state.D, v);
			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}