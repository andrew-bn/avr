namespace Emulator.Avr
{
	public class Rjmp: Instruction
	{
		public Rjmp() : base("1100 kkkk kkkk kkkk")
		{
			
		}

		public override void Process(ExecutionState state)
		{
			state.Proc.PC = state.Proc.PC + state.K + 1;

			state.Proc.Tick(2);
		}
	}
}