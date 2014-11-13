namespace Emulator.Avr
{
	public class Inc: Instruction
	{
		public Inc()
			: base("1001 010d dddd 0011")
		{
		}
		
		public override void Process(ExecutionState state)
		{
			var v = state.Proc.RegisterGet((Register)state.D);

			if (v == 0x7F) state.Proc.StatusSet(Status.V);
			else state.Proc.StatusClear(Status.V);

			v++;

			state.Proc.Status(Status.Z,v == 0);
			
			state.Proc.Status(Status.N,(v & 0x80) == 0x80);

			state.Proc.Status(Status.S,state.Proc.StatusGet(Status.V) || state.Proc.StatusGet(Status.N));

			state.Proc.RegisterSet((Register)state.D, v);
			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}