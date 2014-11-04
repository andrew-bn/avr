namespace Emulator.Avr
{
	public class Ldi : Instruction
	{
		public Ldi() : base("1110 KKKK dddd KKKK") { }

		public override void Process(ExecutionState state)
		{
			state.Proc.RegisterSet((Register)(state.D + 0x10), (byte)state.K);
			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}