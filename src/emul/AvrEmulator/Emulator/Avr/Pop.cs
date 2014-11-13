using Microsoft.Win32.SafeHandles;

namespace Emulator.Avr
{
	public class Pop: Instruction
	{
		public Pop() : base("1001 000d dddd 1111")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.MemoryGet(state.Proc.SP);
			state.Proc.RegisterSet((Register)state.D,v);
			state.Proc.MemorySet(state.Proc.SP, v);
			state.Proc.SP++;
			state.Proc.PC++;

			state.Proc.Tick(2);
		}
	}
}