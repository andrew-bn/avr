using Microsoft.Win32.SafeHandles;

namespace Emulator.Avr
{
	public class In: Instruction
	{
		public In():base("1011 0AAd dddd AAAA")
		{ }
	

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.PortGet(state.A);

			state.Proc.RegisterSet((Register)state.D, v);
			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}