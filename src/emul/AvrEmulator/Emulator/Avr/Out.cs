namespace Emulator.Avr
{
	public class Out: Instruction
	{
		public Out() : base("1011 1AAr rrrr AAAA")
		{
		}

		public override void Process(long cmd, Processor proc)
		{
			var r = GetInt('r', cmd);
			var a = GetInt('A', cmd);
			proc.PortSet(a, r);

			proc.PC++;

			proc.Tick();
		}

	}
}