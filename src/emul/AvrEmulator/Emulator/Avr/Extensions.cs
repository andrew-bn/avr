namespace Emulator.Avr
{
	public static class Extensions
	{

		public static ushort Merge(this ushort data, ushort mask1, byte shift1, ushort mask2, byte shift2)
		{
			return (ushort)(((data & mask1)>>shift1)|((data & mask2)>>shift2));
		}
		public static uint Merge(this ushort data, ushort mask1, byte shift1, ushort mask2, byte shift2, ushort mask3, byte shift3)
		{
			return (uint)(((data & mask1) >> shift1) | ((data & mask2) >> shift2) | ((data & mask3) >> shift3));
		}

		public static byte High(this int data)
		{
			return (byte) ((data & 0xFF00) >> 8);
		}
		
		public static byte Low(this int data)
		{
			return (byte)(data & 0xFF);
		}
	}
}