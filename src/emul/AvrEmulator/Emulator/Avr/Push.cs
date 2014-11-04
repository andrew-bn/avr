using Microsoft.Win32.SafeHandles;

namespace Emulator.Avr
{
	public class Push: Instruction
	{
		public Push() : base("1001 001d dddd 1111")
		{
		}

		public override void Process(long cmd, Processor proc)
		{
			var d = GetInt('d', cmd);
			var v = proc.RegisterGet((Register)d);
			proc.MemorySet(proc.SP, v);
			proc.SP--;
			proc.PC++;

			proc.Tick(2);
		}

	}
}