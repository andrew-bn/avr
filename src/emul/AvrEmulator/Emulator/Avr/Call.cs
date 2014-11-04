using Microsoft.Win32.SafeHandles;

namespace Emulator.Avr
{
	public class Call: Instruction
	{
		public Call() : base("1001 010k kkkk 111k    kkkk kkkk kkkk kkkk")
		{
			
		}
		public override void Process(ExecutionState state)
		{
			state.Proc.MemorySet(state.Proc.SP, (state.Proc.PC + 2).Low());
			state.Proc.MemorySet(state.Proc.SP - 1, (state.Proc.PC + 2).High());
			state.Proc.SP -= 2;
			state.Proc.PC = state.K;

			state.Proc.Tick(4);
		}
	}
}