namespace Emulator.Avr
{
	public class Movw : Instruction
	{
		public Movw() : base("0000 0001 dddd rrrr") { }

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)(state.R * 2));
			state.Proc.RegisterSet((Register)(state.D * 2),v);

			v = state.Proc.RegisterGet((Register)(state.R*2+1));
			state.Proc.RegisterSet((Register)(state.D*2+1), v);

			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}