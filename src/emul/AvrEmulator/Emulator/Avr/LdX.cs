namespace Emulator.Avr
{
	public class LdX : Instruction
	{
		public LdX() : base("1001 000d dddd 1100") { }

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.MemoryGet(state.Proc.X);
			state.Proc.RegisterSet((Register) state.D, v);

			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class LdXInc : Instruction
	{
		public LdXInc() : base("1001 000d dddd 1101") { }

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.MemoryGet(state.Proc.X);
			state.Proc.RegisterSet((Register)state.D, v);
			state.Proc.X++;
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}

	public class LdDecX : Instruction
	{
		public LdDecX() : base("1001 000d dddd 1110") { }

		public override void Process(ExecutionState state)
		{
			state.Proc.X--;
			var v = state.Proc.MemoryGet(state.Proc.X);
			state.Proc.RegisterSet((Register)state.D, v);
			
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}
}