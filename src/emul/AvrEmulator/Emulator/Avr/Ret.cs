using Microsoft.Win32.SafeHandles;

namespace Emulator.Avr
{
	public class Ret: Instruction
	{
		public Ret() : base("1001 0101 0000 1000")
		{
			
		}
		public override void Process(ExecutionState state)
		{
			state.Proc.PC = (state.Proc.MemoryGet(state.Proc.SP+1) << 8) | state.Proc.MemoryGet(state.Proc.SP+2);
			state.Proc.SP += 2;
			state.Proc.Tick(4);
		}
	}
}