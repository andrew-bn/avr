namespace Emulator.Avr
{
	public class Sbr: Instruction
	{
		public Sbr(): base("0110 kkkk dddd kkkk")
		{
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)( state.D + 0x10));

			v |= (byte)state.K;
			state.Proc.RegisterSet((Register) (state.D+0x10),v);
			state.Proc.Status(Status.Z, v == 0);
			state.Proc.StatusClear(Status.V);
			state.Proc.Status(Status.N, (v & 0x80) != 0);
			state.Proc.Status(Status.S, (v & 0x80) != 0);

			state.Proc.PC++;
			state.Proc.Tick();
		}
	}
}