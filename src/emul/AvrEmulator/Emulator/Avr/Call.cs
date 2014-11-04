using Microsoft.Win32.SafeHandles;

namespace Emulator.Avr
{
	public class Call: Instruction
	{
		public override bool Process(Processor proc)
		{
			var inst = proc.Instruction;
			if ((inst & 0xFE0E) != 0x940E)
				return false;

			var d = inst.Merge(0x100,3,0xf0,3,0x1,0);
			var k = (d<<16)| (proc.LInstruction&0xFFFF);
			
			proc.MemorySet(proc.SP, (proc.PC+2).Low());
			proc.MemorySet(proc.SP-1, (proc.PC + 2).High());
			proc.SP -= 2;
			proc.PC = (int)k;
			
			proc.Tick(4);

			return true;
		}

	}
}