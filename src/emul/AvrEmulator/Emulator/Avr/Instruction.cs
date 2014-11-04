using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Emulator.Avr
{
	public abstract class Instruction
	{
		protected Instruction()
		{
			CmdMask = -1l;
		}
		public long CmdMask { get; private set; }
		public long CmdCode { get; private set; }
		public string Pattern { get; private set; }
		private Dictionary<char, Dictionary<long, int>> _patternMap;
		public int CommandLength { get; private set; }
		protected Instruction(string pattern)
		{
			Pattern = pattern.Replace(" ", "");
			CommandLength = Pattern.Length/8;
			CmdMask = 0;
			for (int i = 0; i < Pattern.Length; i++)
			{
				if (Pattern[Pattern.Length - i - 1] == '1')
					CmdCode |= 1 << i;
				if (char.IsDigit(Pattern[Pattern.Length - i - 1]))
					CmdMask |= 1 << i;
			}

			_patternMap = Pattern.GroupBy(c => c)
				.Where(g => !char.IsDigit(g.Key))
				.ToDictionary(g => g.Key, g => Check(g.Key, Pattern));
		}

		public int GetInt(char p, long cmd)
		{
			var r = 0;
			foreach (var pr in _patternMap[p])
			{
				r |= (int)((cmd & pr.Key) >> pr.Value);
			}
			return r;
		}
		public static Dictionary<long, int> Check(char p, string cmd)
		{
			var res = new Dictionary<long, int>();
			int shiftTo = 0;
			long shiftPattern = 0;
			for(int i = 0;i<cmd.Length;i++)
			{
				var c = cmd[cmd.Length - i - 1];
				if (c != p && shiftPattern != 0)
				{
					res.Add(shiftPattern, shiftTo);
					shiftPattern = 0;
				}
				if (c != p)
					shiftTo++;
				else
					shiftPattern |= (1 << i);
			}
			if (shiftPattern != 0)
			{
				res.Add(shiftPattern, shiftTo);
			}
			return res;
		}

		public bool Run(Processor proc)
		{
			long cmd = proc.Flash[proc.PC];
			for (int i = 2; i < CommandLength; i += 2)
				cmd = (cmd << 16) | proc.Flash[proc.PC + i/2];
			if ((cmd & CmdMask) != CmdCode)
				return false;
			Process(cmd, proc);
			return true;
		}

		public virtual void Process(long cmd, Processor proc)
		{
			
		}

		public virtual bool Process(Processor proc)
		{
		}
	}

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