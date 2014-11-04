namespace Emulator.Avr
{
	public class Subi: Instruction
	{
		public Subi() : base("0101 KKKK dddd KKKK")
		{
			
		}

		public override void Process(ExecutionState state)
		{
			state.Proc.RegisterSet((Register)state.D, 0);
			state.Proc.StatusClear(Status.N);
			state.Proc.StatusClear(Status.V);
			state.Proc.StatusClear(Status.S);
			state.Proc.StatusSet(Status.Z);
			state.Proc.PC++;

			state.Proc.Tick();

		}
	}
}