namespace Emulator.Avr
{
	public class StY: Instruction
	{
		public StY() : base("1000 001r rrrr 1000")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register) state.R);
			state.Proc.MemorySet(state.Proc.Y, v);

			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class StYInc : Instruction
	{
		public StYInc()
			: base("1001 001r rrrr 1001")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)state.R);
			state.Proc.MemorySet(state.Proc.Y, v);
			state.Proc.Y++;
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class StDecY : Instruction
	{
		public StDecY()
			: base("1001 001r rrrr 1010")
		{
		}

		public override void Process(ExecutionState state)
		{
			state.Proc.Y--;
			var v = state.Proc.RegisterGet((Register)state.R);
			state.Proc.MemorySet(state.Proc.Y, v);
		
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}
	public class StdY : Instruction
	{
		public StdY()
			: base("10k0 kk1r rrrr 1kkk")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)state.R);
			state.Proc.MemorySet(state.Proc.Y+state.K, v);

			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}
}