using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Emulator.Avr
{
	public class ExecutionState
	{
		public Instruction Instruction { get; set; }
		public ulong Command { get; set; }
		public Processor Proc { get; set; }
		public int A { get; set; }
		public int D { get; set; }
		public int R { get; set; }
		public int K { get; set; }
		public int B { get; set; }
		public ExecutionState(Instruction instruction, ulong command, Processor proc, int a, int d, int r, int k, int b)
		{
			Instruction = instruction;
			Command = command;
			Proc = proc;
			A = a;
			D = d;
			R = r;
			K = k;
			B = b;
		}

		public void Execute()
		{
			Instruction.Process(this);
		}
	}
	public abstract class Instruction
	{
		protected Instruction()
		{
		}
		public ulong CmdMask { get; private set; }
		public ulong CmdCode { get; private set; }
		public string Pattern { get; private set; }
		private Dictionary<char, Dictionary<ulong, int>> _patternMap;
		public int CommandLength { get; private set; }
		protected Instruction(string pattern)
		{
			Pattern = pattern.ToLower().Replace(" ", "");
			CommandLength = Pattern.Length/8;
			CmdMask = 0;
			for (int i = 0; i < Pattern.Length; i++)
			{
				if (Pattern[Pattern.Length - i - 1] == '1')
					CmdCode |= (ulong)(1l << i);
				if (char.IsDigit(Pattern[Pattern.Length - i - 1]))
					CmdMask |= (ulong)(1l << i);
			}

			_patternMap = Pattern.GroupBy(c => c)
				.Where(g => !char.IsDigit(g.Key))
				.ToDictionary(g => g.Key, g => Check(g.Key, Pattern));
		}

		public int GetInt(char p, ulong cmd)
		{
			var r = 0;
			foreach (var pr in _patternMap[p])
			{
				r |= (int)((cmd & pr.Key) >> pr.Value);
			}
			return r;
		}
		public static Dictionary<ulong, int> Check(char p, string cmd)
		{
			var res = new Dictionary<ulong, int>();
			int shiftTo = 0;
			ulong shiftPattern = 0;
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
					shiftPattern |= (ulong)(1 << i);
			}
			if (shiftPattern != 0)
			{
				res.Add(shiftPattern, shiftTo);
			}
			return res;
		}

		public ExecutionState GetExecutionState(Processor proc, int address)
		{
			ulong cmd = proc.Flash[address].Cell;
			for (int i = 2; i < CommandLength; i += 2)
				cmd = (cmd << 16) | proc.Flash[address + i / 2].Cell;
			if ((cmd & CmdMask) != CmdCode)
				return null;
			int a = 0, d = 0, r = 0, k = 0, b = 0;
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
				else if (p.Key == 'b')
					b = GetInt('b', cmd);
			}
			return new ExecutionState(this, cmd, proc, a, d, r, k,b);

		}

		public abstract void Process(ExecutionState state);

	}
}