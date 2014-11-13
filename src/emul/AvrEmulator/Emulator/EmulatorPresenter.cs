using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
	public enum RecordType
	{
		Bin = 0,
		Eof,
		Seg,
		StartSeg,
		ExtAddr,
		StartLin,
	}
	public class EmulatorPresenter
	{
		private readonly IEmulatorUI _ui;
		private string _asmFile;
		private Processor _processor;
		private Dictionary<string, int> _labelsMap;
		private Dictionary<string, int> _equMap;
		private Dictionary<string, Register> _definitionsMap;
		private Dictionary<int, int> _addressToSourceMap;
		private Dictionary<int, int> _sourceToAddressMap;
		private List<int> _breakpoints = new List<int>();
		public EmulatorPresenter(IEmulatorUI ui)
		{
			_ui = ui;
			LoadTypeItems();
			LoadObjectViewers();
		}

		private void LoadTypeItems()
		{
			var files = Directory.GetFiles(Environment.CurrentDirectory, "*.ti");
			var sb = new StringBuilder();
			foreach (var f in files)
			{
				sb.AppendLine(File.ReadAllText(f));
			}

			var objectViewerMv = sb.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			TypeItem type = null;
			List<PropertyItem> props = null;
			foreach (var line in objectViewerMv)
			{
				var trimmed = line.Trim();
				if (trimmed.StartsWith("["))
				{
					if (type != null)
					{
						type.Properties = props.ToArray();
						_ui.AddViewerType(type);
					}
					type = new TypeItem()
					{
						TypeName = line.Substring(1, line.Length - 2)
					};
					props = new List<PropertyItem>();
				}
				else
				{
					var descr = trimmed.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					props.Add(new PropertyItem()
						{
							Name = string.Join(" ", descr.Skip(1)),
							Type = descr[0]
						});
				}
			}
			if (type != null)
			{
				type.Properties = props.ToArray();
				_ui.AddViewerType(type);
			}
		}

		private void LoadObjectViewers()
		{
			var files = Directory.GetFiles(Environment.CurrentDirectory, "*.mv");
			var sb = new StringBuilder();
			foreach (var f in files)
			{
				sb.AppendLine(File.ReadAllText(f));
			}

			var objectViewerMv = sb.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			var viewer = string.Empty;
			List<ObjectItem> objects = null;
			foreach (var line in objectViewerMv)
			{
				var trimmed = line.Trim();
				if (trimmed.StartsWith("["))
				{
					if (!string.IsNullOrEmpty(viewer))
					{
						_ui.CreateViewer(viewer, objects.ToArray());
					}
					viewer = line.Substring(1, line.Length - 2);
					objects = new List<ObjectItem>();
				}
				else
				{
					var descr = trimmed.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					objects.Add(new ObjectItem(int.Parse(descr[0]), string.Join(" ", descr.Skip(2)), descr[1]));
				}
			}
			if (!string.IsNullOrEmpty(viewer))
			{
				_ui.CreateViewer(viewer, objects.ToArray());
			}
		}
		internal void Reset()
		{
			_processor.Reset();
			_ui.JumpToLine(MapAddressToLine(_processor.PC));
		}
		internal void Load(string fileName)
		{

			var dir = Path.GetDirectoryName(fileName);
			var file = Path.GetFileNameWithoutExtension(fileName);
			_asmFile = File.ReadAllText(dir + "\\" + file + ".lss").Replace("\r\n", "\n");
			_addressToSourceMap = LoadAddressToSourceMap(File.ReadAllLines(dir + "\\" + file + ".lss"));
			_sourceToAddressMap = _addressToSourceMap.ToDictionary(i => i.Value, i => i.Key);
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

			_processor = new Processor(8000000, ReadFlash(File.ReadAllLines(dir + "\\" + file + ".hex")));
			_processor.FlashChanged += _processor_FlashChanged;
			_ui.LoadAsmContent(new LoadContentArgs(_processor, _asmFile, _labelsMap, _definitionsMap, _equMap));

			_ui.JumpToLine(MapAddressToLine(0));
		}
		private Dictionary<int, byte> _affectedAddresses = new Dictionary<int, byte>();
		void _processor_FlashChanged(int address, byte value)
		{
			if (_affectedAddresses.ContainsKey(address))
				_affectedAddresses[address] = value;
			else _affectedAddresses.Add(address, value);
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
			_affectedAddresses.Clear();
			var prevSp = _processor.SP;
			_processor.Step();
			if (_processor.SP != prevSp)
				_ui.HighlightStackPointer(_processor.SP);
			_ui.JumpToLine(MapAddressToLine(_processor.PC));
			_ui.RefreshProcessorStatus(_processor.Ticks, _processor.Frequency);
			_ui.RefreshAddress(_affectedAddresses);
		}


		internal void SetBreakpointOnLine(int line)
		{
			if (_sourceToAddressMap.ContainsKey(line))
			{
				if (!_breakpoints.Contains(_sourceToAddressMap[line]))
				{
					_breakpoints.Add(_sourceToAddressMap[line]);
					_ui.SetBreakpoint(line);
				}
				else
				{
					_breakpoints.Remove(_sourceToAddressMap[line]);
					_ui.RemoveBreakpoint(line);
				}
			}
		}

		internal void Run()
		{
			_affectedAddresses.Clear();
			while (true)
			{
				_processor.Step();
				if (_breakpoints.Contains(_processor.PC))
				{
					_ui.HighlightStackPointer(_processor.SP);
					_ui.JumpToLine(MapAddressToLine(_processor.PC));
					_ui.RefreshProcessorStatus(_processor.Ticks, _processor.Frequency);
					_ui.RefreshAddress(_affectedAddresses);

					break;
				}
			}
		}


	}
}
