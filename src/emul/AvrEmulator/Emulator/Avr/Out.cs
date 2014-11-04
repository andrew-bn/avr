namespace Emulator.Avr
{
	public class Out: Instruction
	{
		public Out() : base("1011 1AAr rrrr AAAA")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register) state.R);
			state.Proc.PortSet(state.A, v);

			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}