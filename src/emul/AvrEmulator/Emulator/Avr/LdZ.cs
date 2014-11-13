namespace Emulator.Avr
{
	public class LdZ : Instruction
	{
		public LdZ() : base("1000 000d dddd 0000") { }

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.MemoryGet(state.Proc.Z);
			state.Proc.RegisterSet((Register) state.D, v);

			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class LdZInc : Instruction
	{
		public LdZInc() : base("1001 000d dddd 0001") { }

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.MemoryGet(state.Proc.Z);
			state.Proc.RegisterSet((Register)state.D, v);
			state.Proc.Z++;
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class LdDecZ : Instruction
	{
		public LdDecZ() : base("1001 000d dddd 0010") { }

		public override void Process(ExecutionState state)
		{
			state.Proc.Z--;
			var v = state.Proc.MemoryGet(state.Proc.Z);
			state.Proc.RegisterSet((Register)state.D, v);
			
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class LddZ : Instruction
	{
		public LddZ() : base("10k0 kk0d dddd 0kkk") { }

		public override void Process(ExecutionState state)
		{
			
			var v = state.Proc.MemoryGet(state.Proc.Z+state.K);
			state.Proc.RegisterSet((Register)state.D, v);

			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}
}