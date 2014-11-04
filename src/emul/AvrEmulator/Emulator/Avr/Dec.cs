namespace Emulator.Avr
{
	public class Dec: Instruction
	{
		public override bool Process(Processor proc)
		{
			var inst = proc.Instruction;
			if ((inst & 0xFE0F) != 0x940A)
				return false;

			var d = inst.Merge(0x100,5,0xf0,4);
			var regVal = proc.RegisterGet((Register)d);

			proc.StatusClear(Status.V);
			if (regVal == 0x80)
				proc.StatusSet(Status.V);

			regVal --;

			proc.StatusClear(Status.N);
			if ((regVal & 0x80)==0x80)
				proc.StatusSet(Status.N);
			
			proc.StatusClear(Status.S);
			if (proc.StatusGet(Status.V) || proc.StatusGet(Status.N))
				proc.StatusSet(Status.S);
			
			proc.RegisterSet((Register)d,regVal);
			proc.PC++;

			proc.Tick();

			return true;
		}

	}
}