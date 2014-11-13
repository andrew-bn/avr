using Microsoft.Win32.SafeHandles;

namespace Emulator.Avr
{
	public class Jmp : Instruction
	{
		public Jmp(): base("1001 010k kkkk 110k    kkkk kkkk kkkk kkkk")
		{

		}
		public override void Process(ExecutionState state)
		{
			state.Proc.PC = state.K;
			state.Proc.Tick(3);
		}
	}
}