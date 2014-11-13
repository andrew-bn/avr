namespace Emulator.Avr
{
	public class Brcc: Instruction
	{
		public Brcc()
			: base("1111 01kk kkkk k000")
		{
			
		}

		public override void Process(ExecutionState state)
		{
			int increment = 1;
			if (!state.Proc.StatusGet(Status.C))
			{
				increment = state.K;
				if ((state.K & 0x40) != 0)
					increment = (int)(state.K | 0xFFFFFF80);
				increment++;

				state.Proc.Tick();
			}
			state.Proc.PC = state.Proc.PC + increment;

			state.Proc.Tick();
		}
	}
}