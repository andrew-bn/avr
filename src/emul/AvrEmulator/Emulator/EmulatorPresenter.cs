using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Emulator.Avr;

namespace Emulator
{
	public class EmulatorPresenter
	{
		private readonly IEmulatorUI _ui;
		private string _asmFile;
		private Processor _processor;
		private Dictionary<string, int> _labelsMap;
		private Dictionary<string, int> _equMap;
		private Dictionary<string, Register> _definitionsMap;
		private Dictionary<int, int> _addressToSourceMap;
		public EmulatorPresenter(IEmulatorUI ui)
		{
			_ui = ui;
		}

		internal void Load(string fileName)
		{
			var dir = Path.GetDirectoryName(fileName);
			var file = Path.GetFileNameWithoutExtension(fileName);
			_asmFile = File.ReadAllText(dir+"\\"+file+".lss").Replace("\r\n", "\n");
			_addressToSourceMap = LoadAddressToSourceMap(File.ReadAllLines(dir + "\\" + file + ".lss"));
			var _map = File.ReadAllLines(dir + "\\" + file + ".map");
			_labelsMap = _map.Where(l => l.StartsWith("CSEG"))
							.Select(l => l.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
							.ToDictionary(l => l[1], l => int.Parse(l[2], NumberStyles.HexNumber));;
			_definitionsMap = _map.Where(l => l.StartsWith("DEF"))
								.Select(l => l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
								.ToDictionary(l => l[1], l => (Register)Enum.Parse(typeof(Register),l[2],true));
			_equMap = _map.Where(l => l.StartsWith("EQU"))
							.Select(l => l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
							.ToDictionary(l => l[1], l => int.Parse(l[2], NumberStyles.HexNumber)); ;

			_processor = new Processor(ReadFlash(File.ReadAllLines(dir + "\\" + file + ".hex")));
			_ui.LoadAsmContent(new LoadContentArgs(_processor, _asmFile,_labelsMap,_definitionsMap,_equMap));

			
			_ui.JumpToLine(MapAddressToLine(0));
		}

		private int MapAddressToLine(int address)
		{
			return _addressToSourceMap[address];
		}

		private Dictionary<int, int> LoadAddressToSourceMap(string[] lines)
		{
			var index = new Dictionary<int, int>();
			var r = new Regex(@"\b\d\b");
			for (int i = 0;i<lines.ToArray().Length;i++)
			{
				if (lines[i].Length<6)continue;
				
				var s = lines[i].Substring(0, 6);
				int addr = 0;
				if (int.TryParse(s,NumberStyles.HexNumber,CultureInfo.InvariantCulture,out addr))
					index.Add(addr, i);
			}

			return index;
		}

		private ushort[] ReadFlash(string[] lines)
		{

			var result = new UInt16[(32 * 1024) / 2];
			foreach (var line in lines)
			{
				if (!line.StartsWith(":")) continue;
				var len = int.Parse(line.Substring(1, 2), NumberStyles.HexNumber);
				var address = int.Parse(line.Substring(3, 4), NumberStyles.HexNumber) / 2;
				var type = (RecordType)int.Parse(line.Substring(7, 2), NumberStyles.HexNumber);
				var data = new UInt16[len / 2];
				for (int i = 0; i < len * 2; i += 4)
				{
					var b1 = (byte)int.Parse(line.Substring(9 + i, 2), System.Globalization.NumberStyles.HexNumber);
					var b2 = (byte)int.Parse(line.Substring(9 + i + 2, 2), System.Globalization.NumberStyles.HexNumber);

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


		internal void Step()
		{
			_processor.Step();
			_ui.JumpToLine(MapAddressToLine(_processor.PC));

			_ui.RefreshAddress(_processor.AffectedAddresses.ToDictionary(a=>a,a=>_processor.Ram[a]));
		}
	}
}
