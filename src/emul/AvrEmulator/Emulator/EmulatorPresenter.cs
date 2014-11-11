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
	/*
	 
	 [TypeName]
	 ParamOffcet ParamName ParamType
	 [NextTypeName]

	 */
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
			_asmFile = File.ReadAllText(dir + "\\" + file + ".lss").Replace("\r\n", "\n");
			_addressToSourceMap = LoadAddressToSourceMap(File.ReadAllLines(dir + "\\" + file + ".lss"));
			var _map = File.ReadAllLines(dir + "\\" + file + ".map");
			_labelsMap = _map.Where(l => l.StartsWith("CSEG"))
							.Select(l => l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
							.ToDictionary(l => l[1], l => int.Parse(l[2], NumberStyles.HexNumber)); ;
			_definitionsMap = _map.Where(l => l.StartsWith("DEF"))
								.Select(l => l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
								.ToDictionary(l => l[1], l => (Register)Enum.Parse(typeof(Register), l[2], true));
			_equMap = _map.Where(l => l.StartsWith("EQU"))
							.Select(l => l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
							.ToDictionary(l => l[1], l => int.Parse(l[2], NumberStyles.HexNumber)); ;

			_processor = new Processor(ReadFlash(File.ReadAllLines(dir + "\\" + file + ".hex")));
			_ui.LoadAsmContent(new LoadContentArgs(_processor, _asmFile, _labelsMap, _definitionsMap, _equMap));
			LoadRegistersView();

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
			for (int i = 0; i < lines.ToArray().Length; i++)
			{
				if (lines[i].Length < 6) continue;

				var s = lines[i].Substring(0, 6);
				int addr = 0;
				if (int.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out addr))
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

			_ui.RefreshAddress(_processor.AffectedAddresses.ToDictionary(a => a, a => _processor.Ram[a]));
		}
		private void LoadRegistersView()
		{
			_ui.LoadView("Registers",
				new ObjectItemAddress(26, "Memory pointers", new TypeItem()
				{
					TypeName = "MemoryPointers",
					Properties = new[]
					{
						new PropertyItem(){Name = "X", Offcet = 0, Type = "Int"}, 
						new PropertyItem(){Name = "Y", Offcet = 2, Type = "Int"}, 
						new PropertyItem(){Name = "Z", Offcet = 4, Type = "Int"}, 
					}
				}),
				new ObjectItemAddress(0, "Registers", new TypeItem()
				{

					TypeName = "Registers",
					Properties = new[]
				{
					new PropertyItem() {Name = "R0", Offcet = 0, Type = "Byte"},
					new PropertyItem() {Name = "R1", Offcet = 1, Type = "Byte"},
					new PropertyItem() {Name = "R2", Offcet = 2, Type = "Byte"},
					new PropertyItem() {Name = "R3", Offcet = 3, Type = "Byte"},
					new PropertyItem() {Name = "R4", Offcet = 4, Type = "Byte"},
					new PropertyItem() {Name = "R5", Offcet = 5, Type = "Byte"},
					new PropertyItem() {Name = "R6", Offcet = 6, Type = "Byte"},
					new PropertyItem() {Name = "R7", Offcet = 7, Type = "Byte"},
					new PropertyItem() {Name = "R8", Offcet = 8, Type = "Byte"},
					new PropertyItem() {Name = "R9", Offcet = 9, Type = "Byte"},
					new PropertyItem() {Name = "R10", Offcet = 10, Type = "Byte"},
					new PropertyItem() {Name = "R11", Offcet = 11, Type = "Byte"},
					new PropertyItem() {Name = "R12", Offcet = 12, Type = "Byte"},
					new PropertyItem() {Name = "R13", Offcet = 13, Type = "Byte"},
					new PropertyItem() {Name = "R14", Offcet = 14, Type = "Byte"},
					new PropertyItem() {Name = "R15", Offcet = 15, Type = "Byte"},
					new PropertyItem() {Name = "R16", Offcet = 16, Type = "Byte"},
					new PropertyItem() {Name = "R17", Offcet = 17, Type = "Byte"},
					new PropertyItem() {Name = "R18", Offcet = 18, Type = "Byte"},
					new PropertyItem() {Name = "R19", Offcet = 19, Type = "Byte"},
					new PropertyItem() {Name = "R20", Offcet = 20, Type = "Byte"},
					new PropertyItem() {Name = "R21", Offcet = 21, Type = "Byte"},
					new PropertyItem() {Name = "R22", Offcet = 22, Type = "Byte"},
					new PropertyItem() {Name = "R23", Offcet = 23, Type = "Byte"},
					new PropertyItem() {Name = "R24", Offcet = 24, Type = "Byte"},
					new PropertyItem() {Name = "R25", Offcet = 25, Type = "Byte"},
					new PropertyItem() {Name = "R26", Offcet = 26, Type = "Byte"},
					new PropertyItem() {Name = "R27", Offcet = 27, Type = "Byte"},
					new PropertyItem() {Name = "R28", Offcet = 28, Type = "Byte"},
					new PropertyItem() {Name = "R29", Offcet = 29, Type = "Byte"},
					new PropertyItem() {Name = "R30", Offcet = 30, Type = "Byte"},
					new PropertyItem() {Name = "R31", Offcet = 31, Type = "Byte"},
				}
				}));
		}
	}
}
