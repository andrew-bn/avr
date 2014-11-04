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
		public Form1()
		{
			InitializeComponent();
			proc = new Processor(ReadHex(@"C:\AB\Work\AVROS\src\clock\Debug\avr.hex"));
			//proc.Step();
			proc.Run();
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
	}
}