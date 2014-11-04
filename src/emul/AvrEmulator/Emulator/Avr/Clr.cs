namespace Emulator.Avr
{
	public class Clr: Instruction
	{
		public Clr() : base("0010 01dd dddd dddd")
		{
		}

		public override void Process(ExecutionState state)
		{
			var d = state.D;
			state.Proc.RegisterSet((Register)d, 0);
			state.Proc.StatusClear(Status.N);
			state.Proc.StatusClear(Status.V);
			state.Proc.StatusClear(Status.S);
			state.Proc.StatusSet(Status.Z);
			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}