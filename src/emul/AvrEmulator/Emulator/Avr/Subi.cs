namespace Emulator.Avr
{
	public class Subi: Instruction
	{
		public override bool Process(Processor proc)
		{
			var inst = proc.Instruction;
			if ((inst & 0xFC00) != 0x2400)
				return false;

			var d = inst.Merge(0x100,5,0xf,0);
			proc.RegisterSet((Register)d,0);
			proc.StatusClear(Status.N);
			proc.StatusClear(Status.V);
			proc.StatusClear(Status.S);
			proc.StatusSet(Status.Z);
			proc.PC++;

			proc.Tick();

			return true;
		}

	}
}