using System;

namespace Emulator.Avr
{
	public class Sbrc: Instruction
	{
		public Sbrc()
			: base("1111 110r rrrr 0bbb")
		{
			
		}

		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register) state.R);
			var nextInstLength = 0;
			if ((v & (1 << state.B)) == 0)
			{
				nextInstLength =  state.Proc.GetInstruction(state.Proc.PC+1).Instruction.CommandLength / 2;
			}
			state.Proc.PC += nextInstLength+1;

			state.Proc.Tick(nextInstLength+1);

		}
	}
}