namespace Emulator.Avr
{
	public class LdY : Instruction
	{
		public LdY() : base("1000 000d dddd 1000") { }

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.MemoryGet(state.Proc.Y);
			state.Proc.RegisterSet((Register) state.D, v);

			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class LdYInc : Instruction
	{
		public LdYInc() : base("1001 000d dddd 1001") { }

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.MemoryGet(state.Proc.Y);
			state.Proc.RegisterSet((Register)state.D, v);
			state.Proc.Y++;
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class LdDecY : Instruction
	{
		public LdDecY() : base("1001 000d dddd 1010") { }

		public override void Process(ExecutionState state)
		{
			state.Proc.Y--;
			var v = state.Proc.MemoryGet(state.Proc.Y);
			state.Proc.RegisterSet((Register)state.D, v);
			
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class LddY : Instruction
	{
		public LddY() : base("10k0 kk0d dddd 1kkk") { }

		public override void Process(ExecutionState state)
		{
			
			var v = state.Proc.MemoryGet(state.Proc.Y+state.K);
			state.Proc.RegisterSet((Register)state.D, v);

			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}
}