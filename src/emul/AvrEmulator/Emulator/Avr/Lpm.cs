namespace Emulator.Avr
{
	public class LpmR0 : Instruction
	{
		public LpmR0() : base("1001 0101 1100 1000") { }

		public override void Process(ExecutionState state)
		{
			var z = state.Proc.Z;
			var cell = state.Proc.Flash[z >> 1].Cell;
			cell = ((z & 0x1) == 0)
						? (ushort)(cell & 0x00FF)
						: (ushort)(cell >> 8);

			state.Proc.RegisterSet(Register.R0, (byte)cell);

			state.Proc.PC++;
			state.Proc.Tick(3);
		}
	}

	public class Lpm : Instruction
	{
		public Lpm() : base("1001 000d dddd 0100") { }

		public override void Process(ExecutionState state)
		{
			var z = state.Proc.Z;
			var cell = state.Proc.Flash[z >> 1].Cell;
			cell = ((z & 0x1) == 0)
						? (ushort)(cell & 0x00FF)
						: (ushort)(cell >> 8);

			state.Proc.RegisterSet((Register)state.D, (byte)cell);

			state.Proc.PC++;
			state.Proc.Tick(3);
		}
	}

	public class LpmZInc : Instruction
	{
		public LpmZInc() : base("1001 000d dddd 0101") { }

		public override void Process(ExecutionState state)
		{
			var z = state.Proc.Z;
			var cell = state.Proc.Flash[z >> 1].Cell;
			cell = ((z & 0x1) == 0)
						? (ushort)(cell & 0x00FF)
						: (ushort)(cell >> 8);

			state.Proc.RegisterSet((Register)state.D, (byte)cell);
			state.Proc.Z++;
			state.Proc.PC++;
			state.Proc.Tick(3);
		}
	}
}