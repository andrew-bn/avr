using Microsoft.Win32.SafeHandles;

namespace Emulator.Avr
{
	public class ICall: Instruction
	{
		public ICall()
			: base("1001 0101 0000 1001")
		{
			
		}
		public override void Process(ExecutionState state)
		{
			state.Proc.MemorySet(state.Proc.SP, (state.Proc.PC + 1).Low());
			state.Proc.MemorySet(state.Proc.SP - 1, (state.Proc.PC + 1).High());
			state.Proc.SP -= 2;
			state.Proc.PC = state.Proc.Z;

			state.Proc.Tick(3);
		}
	}
}