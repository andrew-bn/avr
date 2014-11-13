namespace Emulator.Avr
{
	public class Cli: Instruction
	{
		public Cli()
			: base("1001 0100 1111 1000")
		{
		}

		public override void Process(ExecutionState state)
		{
			state.Proc.StatusClear(Status.I);
			state.Proc.PC++;
			state.Proc.Tick();
		}
	}
}