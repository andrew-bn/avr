namespace Emulator.Avr
{
	public class Sei: Instruction
	{
		public Sei()
			: base("1001 0100 0111 1000")
		{
		}

		public override void Process(ExecutionState state)
		{
			state.Proc.StatusSet(Status.I);
			state.Proc.PC++;
			state.Proc.Tick();
		}
	}
}