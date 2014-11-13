namespace Emulator.Avr
{
	public class Breq: Instruction
	{
		public Breq()
			: base("1111 00kk kkkk k001")
		{
			
		}

		public override void Process(ExecutionState state)
		{
			int increment = 1;
			if (state.Proc.StatusGet(Status.Z))
			{
				increment = state.K;
				if ((state.K & 0x40) != 0)
					increment = (int) (state.K | 0xFFFFFF80);
				increment++;

				state.Proc.Tick();
			}
			state.Proc.PC = state.Proc.PC + increment;

			state.Proc.Tick();
		}
	}
}