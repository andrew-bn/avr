namespace Emulator.Avr
{
	public class Movw : Instruction
	{
		public Movw() : base("0000 0001 dddd rrrr") { }

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)(state.R +0x10));
			state.Proc.RegisterSet((Register)(state.D + 0x10),v);

			v = state.Proc.RegisterGet((Register)(state.R + 0x10+1));
			state.Proc.RegisterSet((Register)(state.D + 0x10+1), v);

			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}