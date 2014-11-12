using System.Globalization;
using System.Text.RegularExpressions;
using Emulator.Avr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Emulator
{
	public enum RecordType
	{
		Bin = 0,
		Eof,
		Seg,
		StartSeg,
		ExtAddr,
		StartLin,
	}
	public partial class Form1 : Form
	{
		Processor proc;
		private Dictionary<int, int> _index;
		public Form1()
		{
			InitializeComponent();
			proc = new Processor(8000000,ReadHex(@"C:\AB\Work\AVROS\src\clock\Debug\avr.hex"));
			//proc.Step();
			//proc.Run();

			LoadSource(File.ReadAllText(@"C:\AB\Work\AVROS\src\clock\Debug\avr.lss"));
			rtb_Source.Select(_index[proc.PC],6);
			rtb_Source.SelectionBackColor = Color.Red;
			DrawMemory();
			var dc = new DockContent();
			dc.Show(dp_Main);
		}

		private void DrawMemory()
		{
			for (int i = 0;i<proc.Ram.Length;i++)
			{
				if (i%16 == 0)
				{
					if (i!=0)
						rtb_Memory.AppendText("\n");
					var curIdx = rtb_Memory.TextLength;
					rtb_Memory.AppendText(string.Format("{0,-5:x4}", i));
					rtb_Memory.Select(curIdx,4);
					rtb_Memory.SelectionBackColor = Color.Black;
					rtb_Memory.SelectionColor = Color.WhiteSmoke;
					rtb_Memory.SelectionLength = 0;
				}
				rtb_Memory.AppendText(string.Format("{0,-3:x2}",proc.Ram[i]));
			}
			
		}

		private void LoadSource(string text)
		{
			text = text.Replace("\r\n","\n");
			_index = new Dictionary<int, int>();
			rtb_Source.Text = text;
			
			var m = Regex.Matches(text, @"\n\d[^\s]+");
			foreach (Match i in m)
			{
				rtb_Source.Select(i.Index,i.Length);
				rtb_Source.SelectionColor = Color.Gray;
				_index.Add(int.Parse(i.Value.Trim(),NumberStyles.HexNumber),i.Index+1);
			}
			
			m = Regex.Matches(text, @"(?<=\n\d[^\s]+)\s[\w]{4}");
			foreach (Match i in m)
			{
				rtb_Source.Select(i.Index, i.Length);
				rtb_Source.SelectionColor = Color.DimGray;
			}
			
			
			m = Regex.Matches(text, @"/\*.*\*/",RegexOptions.Multiline);
			foreach (Match i in m)
			{
				rtb_Source.Select(i.Index, i.Length);
				rtb_Source.SelectionColor = Color.DarkOliveGreen;
			}
			m = Regex.Matches(text, @";[^\n]*");
			foreach (Match i in m)
			{
				rtb_Source.Select(i.Index, i.Length);
				rtb_Source.SelectionColor = Color.DarkOliveGreen;
			}
			m = Regex.Matches(text, @"//[^\n]*");
			foreach (Match i in m)
			{
				rtb_Source.Select(i.Index, i.Length);
				rtb_Source.SelectionColor = Color.DarkOliveGreen;
			}
			m = Regex.Matches(text, @"\n\s*(\.|#)\w+");
			foreach (Match i in m)
			{
				rtb_Source.Select(i.Index, i.Length);
				rtb_Source.SelectionColor = Color.DodgerBlue;
			}
			m = Regex.Matches(text, @"(?<=\+)\w+");
			foreach (Match i in m)
			{
				rtb_Source.Select(i.Index, i.Length);
				rtb_Source.SelectionColor = Color.Black;
				rtb_Source.SelectedText = rtb_Source.SelectedText.ToUpper();
			}
			m = Regex.Matches(text, @"(?<=\n\d[^\s]+\s[\w]{4}\s+)\w+");
			foreach (Match i in m)
			{
				rtb_Source.Select(i.Index, i.Length);
				rtb_Source.SelectionColor = Color.Black;
				rtb_Source.SelectedText = rtb_Source.SelectedText.ToUpper();
			}
			m = Regex.Matches(text, @"\+");
			foreach (Match i in m)
			{
				rtb_Source.Select(i.Index, i.Length);
				rtb_Source.SelectionColor = Color.Transparent;
			}
		}

		private UInt16[] ReadHex(string fileName)
		{

			var result = new UInt16[(32 * 1024) / 2];
			var lines = File.ReadAllLines(fileName);
			foreach(var line in lines)
			{
				if (!line.StartsWith(":")) continue;
				var len = int.Parse(line.Substring(1,2), System.Globalization.NumberStyles.HexNumber);
				var address = int.Parse(line.Substring(3, 4), System.Globalization.NumberStyles.HexNumber)/2;
				var type = (RecordType)int.Parse(line.Substring(7,2), System.Globalization.NumberStyles.HexNumber);
				var data = new UInt16[len / 2];
				for(int i = 0;i<len*2;i+=4)
				{
					var b1 = (byte)int.Parse(line.Substring(9 + i, 2), System.Globalization.NumberStyles.HexNumber);
					var b2 = (byte)int.Parse(line.Substring(9 + i+2, 2), System.Globalization.NumberStyles.HexNumber);

					data[i / 4] = (UInt16)((b2 << 8) | b1);
				}
				
				if (type == RecordType.Bin)
				{
					for (int i = 0; i < data.Length; i++)
					{
						result[i + address] = data[i];
					}
				}
				else if (type == RecordType.Eof)
				{
					break;
				}
			}
			return result;
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			rtb_Source.Select(_index[proc.PC], 6);
			rtb_Source.SelectionBackColor = Color.WhiteSmoke;
			rtb_Source.SelectionLength = 0;
			proc.Step();
			rtb_Source.Select(_index[proc.PC], 6);
			rtb_Source.SelectionBackColor = Color.Red;
			rtb_Source.SelectionLength = 0;

			rtb_Memory.SelectAll();
			rtb_Memory.SelectionColor = Color.WhiteSmoke;
			rtb_Memory.SelectionLength = 0;
			foreach (var addr in proc.AffectedAddresses)
			{
				rtb_Memory.Select(addr * 3 + (addr / 16+1)*5 + addr / 16, 2);
				rtb_Memory.SelectionColor = Color.Yellow;
				rtb_Memory.SelectedText = string.Format("{0,-2:x2}",proc.Ram[addr]);
				rtb_Memory.SelectionLength = 0;
			}
		}

		private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void rtb_Memory_SelectionChanged(object sender, EventArgs e)
		{
			tssl_StartSelection.Text = rtb_Memory.SelectionStart.ToString();
			tssl_SelectionLength.Text = rtb_Memory.SelectionLength.ToString();
		}

		private void rtb_Memory_TextChanged(object sender, EventArgs e)
		{

		}

		private void dockPanel2_ActiveContentChanged(object sender, EventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
		//	dp_Main.
		}

		private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			
		}
	}
}