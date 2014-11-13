namespace Emulator.Avr
{
	public class Rjmp: Instruction
	{
		public Rjmp() : base("1100 kkkk kkkk kkkk")
		{
			
		}

		public override void Process(ExecutionState state)
		{
			int increment = state.K + 1;
			if ((state.K & 0x800) == 0x800)
				increment = -((~state.K)&0xFFF);
			state.Proc.PC = state.Proc.PC + increment;

			state.Proc.Tick(2);
		}
	}
}