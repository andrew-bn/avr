using System.Collections.Generic;
using System.Drawing;
using Emulator.Avr;
using FastColoredTextBoxNS;
using WeifenLuo.WinFormsUI.Docking;

namespace Emulator
{
	public partial class Memory : DockContent
	{
		readonly TextStyle _changedStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);
		readonly TextStyle _highlightStyle = new TextStyle(Brushes.SteelBlue, null, FontStyle.Regular);
		private readonly int _changedStyleIndex;
		public Memory(string text)
		{
			InitializeComponent();
			Text = text;
			_changedStyleIndex = rtb_Memory.AddStyle(_changedStyle);
			rtb_Memory.AddStyle(_highlightStyle);
		}

		public void Load(Processor proc)
		{
			DrawMemory(proc);
		}
		private void DrawMemory(Processor proc)
		{
			rtb_Memory.TextSource.Clear();
			Line newLine = null;
			for (int i = 0; i < proc.Ram.Length; i++)
			{
				if (i % 16 == 0)
				{
					if (newLine!=null)
						rtb_Memory.TextSource.Add(newLine);
					newLine = rtb_Memory.TextSource.CreateLine();
				}
				foreach (var c in string.Format("{0,-3:x2}", proc.Ram[i]))
				{
					newLine.Add(new Char(c));
				}
			}
			if (newLine != null)
				rtb_Memory.TextSource.Add(newLine);
		}

		internal void RefreshAddress(Dictionary<int,byte> addressValueMap)
		{
			var mask = rtb_Memory.GetStyleIndexMask(new[] { _changedStyle });
			rtb_Memory.ClearStyle(mask);
			foreach (var item in addressValueMap)
			{
				var address = item.Key;
				var value = item.Value;
				var newVal = string.Format("{0,-3:x2}", value);

				rtb_Memory.TextSource[address/16][(address - address/16*16)*3] = new Char(newVal[0]) {style = mask};
				rtb_Memory.TextSource[address/16][(address - address/16*16)*3 + 1] = new Char(newVal[1]) {style = mask};
			}
			rtb_Memory.Invalidate();
		}

		internal void Highlight(int address)
		{
			var mask = rtb_Memory.GetStyleIndexMask(new[] { _highlightStyle });
			rtb_Memory.ClearStyle(mask);
	
			rtb_Memory.TextSource[address / 16][(address - address / 16 * 16) * 3] =
				new Char(rtb_Memory.TextSource[address / 16][(address - address / 16 * 16) * 3].c) { style = mask };
				rtb_Memory.TextSource[address / 16][(address - address / 16 * 16) * 3 + 1] =
					new Char(rtb_Memory.TextSource[address / 16][(address - address / 16 * 16) * 3 + 1].c) { style = mask };
			
			rtb_Memory.Invalidate();
		}
	}
}
