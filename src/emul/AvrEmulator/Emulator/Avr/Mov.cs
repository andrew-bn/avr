namespace Emulator.Avr
{
	public class Mov : Instruction
	{
		public Mov() : base("0010 11rd dddd rrrr") { }

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)state.R);
			state.Proc.RegisterSet((Register)state.D,v);

			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}