using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emulator.Avr;

namespace Emulator
{
	public class LoadContentArgs
	{
		public Processor Processor { get; private set; }
		public string Content { get; private set; }
		public Dictionary<string, int> LabelsMap { get; private set; }
		public Dictionary<string, Register> DefinitionsMap { get; private set; }
		public Dictionary<string, int> EquMap { get; private set; }

		public LoadContentArgs(Processor processor, string content, Dictionary<string, int> labelsMap,
			Dictionary<string, Register> definitionsMap, Dictionary<string, int> equMap)
		{
			Processor = processor;
			Content = content;
			LabelsMap = labelsMap;
			DefinitionsMap = definitionsMap;
			EquMap = equMap;
		}
	}
}
