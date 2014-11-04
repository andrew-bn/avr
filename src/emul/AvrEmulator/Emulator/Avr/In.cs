using Microsoft.Win32.SafeHandles;

namespace Emulator.Avr
{
	public class In: Instruction
	{
		public override bool Process(Processor proc)
		{
			var inst = proc.Instruction;
			if ((inst & 0xF800) != 0xB000)
				return false;

			var d = inst.Merge(0x100,4,0xf0,4);
			var a = inst.Merge(0x600, 5, 0xf, 0);

			var v = proc.PortGet(a);

			proc.RegisterSet((Register)d,v);
			proc.PC++;

			proc.Tick();

			return true;
		}

	}
}