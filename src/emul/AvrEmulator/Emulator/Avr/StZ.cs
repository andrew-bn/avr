namespace Emulator.Avr
{
	public class StZ: Instruction
	{
		public StZ() : base("1000 001r rrrr 0000")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register) state.R);
			state.Proc.MemorySet(state.Proc.Z, v);

			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class StZInc : Instruction
	{
		public StZInc()
			: base("1001 001r rrrr 0001")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)state.R);
			state.Proc.MemorySet(state.Proc.Z, v);
			state.Proc.Z++;
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class StDecZ : Instruction
	{
		public StDecZ()
			: base("1001 001r rrrr 0010")
		{
		}

		public override void Process(ExecutionState state)
		{
			state.Proc.Z--;
			var v = state.Proc.RegisterGet((Register)state.R);
			state.Proc.MemorySet(state.Proc.Z, v);
		
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class StdZ : Instruction
	{
		public StdZ()
			: base("10k0 kk1r rrrr 0kkk")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)state.R);
			state.Proc.MemorySet(state.Proc.Z+state.K, v);

			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}
}