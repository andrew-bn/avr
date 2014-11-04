using Microsoft.Win32.SafeHandles;

namespace Emulator.Avr
{
	public class Push: Instruction
	{
		public Push() : base("1001 001d dddd 1111")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)state.D);
			state.Proc.MemorySet(state.Proc.SP, v);
			state.Proc.SP--;
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}
}