using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using WeifenLuo.WinFormsUI.Docking;

namespace Emulator
{

	public partial class AsmContent : DockContent
	{

		readonly TextStyle _instructionStyle = new TextStyle(Brushes.MidnightBlue, null, FontStyle.Regular);
		readonly TextStyle _addressStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
		readonly TextStyle _commentStyle = new TextStyle(Brushes.Green, null, FontStyle.Regular);
		readonly TextStyle _dirStyle = new TextStyle(Brushes.Brown, null, FontStyle.Regular);
		readonly TextStyle _macroParameterStyle = new TextStyle(Brushes.CadetBlue, null, FontStyle.Regular);
		readonly TextStyle _labelStyle = new TextStyle(Brushes.Black, null, FontStyle.Underline);
		private LoadContentArgs _content;

		public event Action<int> LineDoubleClick;

		public AsmContent()
		{
			InitializeComponent();
		}

		public void Load(LoadContentArgs args)
		{
			_content = args;
			rtb_Source.Text = args.Content;
		}

		private void rtb_Source_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
		{
			if (rtb_Source.Text == "") return;
			e.ChangedRange.ClearStyle(_instructionStyle);
			e.ChangedRange.SetStyle(_instructionStyle, @"\b(lds|sts|adc|add|adiw|and|andi|asr|bclr|bld|brbc|brbs|brcc|brcs|break|breq|brge|brhc|brhs|brid|brie|brlo|brlt|brmi|brne|brpl|brsh|brtc|brts|brvc|brvs|bset|bst|call|cbi|cbr|clc|clh|cli|cln|clr|cls|clt|clv|clz|com|cp|cpc|cpi|cpse|dec|eor|fmul|fmuls|fmulsu|icall|ijmp|in|inc|jmp|ld|ldd|ldi|lds|lpm|lsl|lsr|mov|movw|mul|muls|mulsu|neg|nop|or|ori|out|pop|push|rcall|ret|reti|rjmp|rol|ror|sbc|sbci|sbi|sbic|sbis|sbiw|sbr|sbrc|sbrs|sec|seh|sei|sen|ser|ses|set|sev|sez|sleep|spm|st|std|sts|sub|subi|swap|tst|wdr)\b", RegexOptions.IgnoreCase);

			e.ChangedRange.ClearStyle(_addressStyle);
			e.ChangedRange.SetStyle(_addressStyle, @"\n\d[^\s]+");
			
			e.ChangedRange.ClearStyle(_commentStyle);
			e.ChangedRange.SetStyle(_commentStyle, @"(/\*.*\*/)|(;[^\n]*)|(//[^\n]*)");

			e.ChangedRange.ClearStyle(_dirStyle);
			e.ChangedRange.SetStyle(_dirStyle, @"(\.device|\.def|\.equ|\.macro|\.include|\.cseg|\.db|\.dw|\.listmac|\.endmacro|\.endm|\.eseg|\.dseg|\.byte|\.org|\.list|\.nolist|\.exit|\.if|\.ifdef|\.endif|\.elif|\.ifndef|\.message|\.error)\b",RegexOptions.IgnoreCase);

			e.ChangedRange.ClearStyle(_macroParameterStyle);
			e.ChangedRange.SetStyle(_macroParameterStyle, @"@\d+", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(_macroParameterStyle, "(\b" + string.Join(@"(?=[\s|:|,|\+]))|(\b", _content.DefinitionsMap.Keys) + ")", RegexOptions.IgnoreCase);

			e.ChangedRange.ClearStyle(_labelStyle);
			e.ChangedRange.SetStyle(_labelStyle, "("+string.Join(@"[\s|:|,])|(",_content.LabelsMap.Keys)+")", RegexOptions.IgnoreCase);
		}

		private int? debuggingLine = 0;
		internal void JumpToLine(int line)
		{
			rtb_Source.Navigate(line);
			debuggingLine = line;
			rtb_Source.CurrentLineColor = Color.Plum;
		}

		private void rtb_Source_PaintLine(object sender, PaintLineEventArgs e)
		{
			if (_breakpoints.Contains(e.LineIndex))
				e.Graphics.DrawRectangle(new Pen(Color.Red, 4), 5, e.LineRect.Y, 45, e.LineRect.Height);
			if (debuggingLine.HasValue && e.LineIndex == debuggingLine.Value)
				e.Graphics.DrawRectangle(new Pen(Color.Red, 4), e.LineRect);
		}

		private void rtb_Source_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			var lineIdx = rtb_Source.Selection.Start.iLine;
			if (LineDoubleClick != null)
				LineDoubleClick(lineIdx);
		}
		List<int> _breakpoints = new List<int>();
		internal void SetBreakpointMarker(int line)
		{
			if (!_breakpoints.Contains(line))
			{
				_breakpoints.Add(line);
				rtb_Source.Refresh();
			}
		}
		internal void RemoveBreakpointMarker(int line)
		{
			if (_breakpoints.Contains(line))
			{
				_breakpoints.Remove(line);
				rtb_Source.Refresh();
			}
		}
	}
}

