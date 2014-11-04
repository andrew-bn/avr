namespace Emulator.Avr
{
	public class Rjmp: Instruction
	{
		public Rjmp() : base("1100 kkkk kkkk kkkk")
		{
			
		}

		public override void Process(long cmd, Processor proc)
		{
			var k = GetInt('k', cmd);
			if ((k & 0x800) == 0x800)
				k = -k;

			proc.PC = proc.PC + k + 1;

			proc.Tick(2);
		}

	}
}