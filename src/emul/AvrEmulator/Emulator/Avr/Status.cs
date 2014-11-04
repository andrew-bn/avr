namespace Emulator.Avr
{
	public enum Status
	{
		C = 0,//C Carry flag. This is a borrow flag on subtracts.
		Z,//Z Zero flag. Set to 1 when an arithmetic result is zero.
		N,
		V,
		S,
		H,
		T,
		I
	}
}