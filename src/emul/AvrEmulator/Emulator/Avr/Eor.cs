namespace Emulator.Avr
{
	public class Eor: Instruction
	{
		public Eor() : base("0010 01rd dddd rrrr")
		{
		}

		public override void Process(ExecutionState state)
		{
			var d = state.Proc.RegisterGet((Register)state.D);
			var r = state.Proc.RegisterGet((Register)state.R);

			d = (byte) (d ^ r);

			state.Proc.RegisterSet((Register)state.D, d);

			state.Proc.Status(Status.Z, d == 0);
			state.Proc.StatusClear(Status.V);
			state.Proc.Status(Status.N, (d & 0x80) == 0x80);
			state.Proc.Status(Status.S, (d & 0x80) == 0x80);
			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}