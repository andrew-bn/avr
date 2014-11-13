namespace Emulator.Avr
{
	public class Add: Instruction
	{
		public Add()
			: base("0000 11rd dddd rrrr")
		{
		}
		
		public override void Process(ExecutionState state)
		{
			var d = state.Proc.RegisterGet((Register)state.D);
			var r = state.Proc.RegisterGet((Register)state.R);

			var res = d + r;
			state.Proc.RegisterSet((Register)state.D,(byte)res);

			state.Proc.Status(Status.C, res>byte.MaxValue);
			state.Proc.Status(Status.Z, res == 0);
			
			state.Proc.StatusClear(Status.V);
			state.Proc.StatusClear(Status.H);

			state.Proc.Status(Status.S,state.Proc.StatusGet(Status.V) ||
				state.Proc.StatusGet(Status.N));

			state.Proc.PC++;

			state.Proc.Tick();
		}
	}
}