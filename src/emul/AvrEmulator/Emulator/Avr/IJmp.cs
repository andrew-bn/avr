using Microsoft.Win32.SafeHandles;

namespace Emulator.Avr
{
	public class IJmp : Instruction
	{
		public IJmp()
			: base("1001 0100 0000 1001")
		{

		}
		public override void Process(ExecutionState state)
		{
			state.Proc.PC = state.Proc.Z;
			state.Proc.Tick(2);
		}
	}
}