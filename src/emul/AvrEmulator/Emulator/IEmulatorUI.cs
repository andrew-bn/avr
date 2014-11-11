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
	}
}
