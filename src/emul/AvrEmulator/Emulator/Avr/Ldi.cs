namespace Emulator.Avr
{
	public class Ldi : Instruction
	{
		public Ldi() : base("1110 KKKK dddd KKKK") { }
		public override void Process(long cmd, Processor proc)
		{
			var k = GetInt('K', cmd);
			var d = GetInt('d', cmd);

			proc.RegisterSet((Register)(d+0x10),(byte)k);
			proc.PC++;

			proc.Tick();
		}

	}
}