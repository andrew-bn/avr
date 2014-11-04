namespace Emulator.Avr
{
	public class Dec: Instruction
	{
		public Dec() : base("1001 010d dddd 1010")
		{
		}
		
		public override void Process(ExecutionState state)
		{
			var regVal = state.Proc.RegisterGet((Register)state.D);

			state.Proc.StatusClear(Status.V);
			if (regVal == 0x80)
				state.Proc.StatusSet(Status.V);

			regVal--;

			state.Proc.StatusClear(Status.N);
			if ((regVal & 0x80) == 0x80)
				state.Proc.StatusSet(Status.N);

			state.Proc.StatusClear(Status.S);
			if (state.Proc.StatusGet(Status.V) || state.Proc.StatusGet(Status.N))
				state.Proc.StatusSet(Status.S);

			state.Proc.RegisterSet((Register)state.D, regVal);
			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}