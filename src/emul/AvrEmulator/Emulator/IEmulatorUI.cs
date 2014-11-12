using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator
{
	public interface IEmulatorUI
	{
		void LoadAsmContent(LoadContentArgs args);

		void JumpToLine(int line);

		void RefreshAddress(Dictionary<int, byte> addressValueMap);

		void CreateViewer(string viewer, ObjectItem[] objectItem);

		void AddViewerType(TypeItem type);

		void RefreshProcessorStatus(long ticks, int frequency);

		void SetBreakpoint(int line);

		void HighlightStackPointer(int address);

		void RemoveBreakpoint(int line);
	}
}
