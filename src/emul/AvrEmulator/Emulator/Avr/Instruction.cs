using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Emulator.Avr
{
	public class ExecutionState
	{
		public long Command { get; set; }
		public Processor Proc { get; set; }
		public int A { get; set; }
		public int D { get; set; }
		public int R { get; set; }
		public int K { get; set; }

		public ExecutionState(long command, Processor proc, int a, int d, int r, int k)
		{
			Command = command;
			Proc = proc;
			A = a;
			D = d;
			R = r;
			K = k;
		}
	}
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
			Pattern = pattern.ToLower().Replace(" ", "");
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
			int a = 0, d = 0, r = 0, k = 0;
			foreach (var p in _patternMap)
			{
				if (p.Key == 'a')
					a = GetInt('a', cmd);
				else if (p.Key == 'd')
					d = GetInt('d', cmd);
				else if (p.Key == 'r')
					r = GetInt('r', cmd);
				else if (p.Key == 'k')
					k = GetInt('k', cmd);
			}
			var state = new ExecutionState(cmd, proc, a, d, r, k);
#if DEBUG
			Log(state);
#endif
			Process(state);

			return true;
		}

		private void Log(ExecutionState state)
		{
			Debug.Write(string.Format("{1,-10:x}{0,-8}   ",GetType().Name.ToUpper(), state.Proc.PC));
			foreach (var p in _patternMap)
			{
				if (p.Key == 'd')
					Debug.Write(string.Format("d:{0,-8:x}   ", state.D));
				else if (p.Key == 'a')
					Debug.Write(string.Format("a:{0,-8:x}   ", state.A));
				else if (p.Key == 'r')
					Debug.Write(string.Format("r:{0,-8:x}   ", state.R));
				else if (p.Key == 'k')
					Debug.Write(string.Format("k:{0,-8:x}   ", state.K));
			}
			Debug.WriteLine("");
		}
		public abstract void Process(ExecutionState state);

	}
}