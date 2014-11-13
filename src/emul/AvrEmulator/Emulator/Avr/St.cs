namespace Emulator.Avr
{
	public class St: Instruction
	{
		public St() : base("1001 001r rrrr 1100")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register) state.R);
			state.Proc.MemorySet(state.Proc.X, v);

			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class StXPlus : Instruction
	{
		public StXPlus()
			: base("1001 001r rrrr 1101")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)state.R);
			state.Proc.MemorySet(state.Proc.X, v);
			state.Proc.X++;
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class StMinX : Instruction
	{
		public StMinX()
			: base("1001 001r rrrr 1110")
		{
		}

		public override void Process(ExecutionState state)
		{
			state.Proc.X--;
			var v = state.Proc.RegisterGet((Register)state.R);
			state.Proc.MemorySet(state.Proc.X, v);
		
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

}