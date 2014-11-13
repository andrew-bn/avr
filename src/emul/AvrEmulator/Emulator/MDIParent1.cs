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
	public partial class MDIParent1 : Form, IEmulatorUI
	{
		private EmulatorPresenter _emulatorPresenter;
		private int childFormNumber = 0;
		private Memory[] _memoryTools;
		private AsmContent _asmContent;
		private List<ObjectViewer> _objectViewers = new List<ObjectViewer>();
		public MDIParent1()
		{
			InitializeComponent();
			_emulatorPresenter = new EmulatorPresenter(this);
		

			_asmContent = new AsmContent();
			_asmContent.Show(dp_Main);

			_memoryTools = new Memory[2];
			_memoryTools[0] = new Memory("Memory1");
			_memoryTools[0].Show(dp_Main, DockState.DockBottom);
			_memoryTools[1] = new Memory("Memory2");
			_memoryTools[1].Show(dp_Main,DockState.DockBottom);

			_asmContent.LineDoubleClick += _asmContent_LineDoubleClick;
		}

		private void ShowNewForm(object sender, EventArgs e)
		{
			Form childForm = new Form();
			childForm.MdiParent = this;
			childForm.Text = "Window " + childFormNumber++;
			childForm.Show();
		}

		private void OpenFile(object sender, EventArgs e)
		{
			if (ofd_OpenHex.ShowDialog(this) == DialogResult.OK)
			{
				var fileName = ofd_OpenHex.FileName;
				OpenFile(fileName);
			}
		}

		private void OpenFile(string fileName)
		{
			
			_emulatorPresenter.Load(fileName);
		}

		void _asmContent_LineDoubleClick(int line)
		{
			_emulatorPresenter.SetBreakpointOnLine(line);
		}

		private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				string FileName = saveFileDialog.FileName;
				
			}
		}

		private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void CutToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStrip.Visible = toolBarToolStripMenuItem.Checked;
		}

		private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			statusStrip.Visible = statusBarToolStripMenuItem.Checked;
		}

		private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}

		private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileVertical);
		}

		private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.ArrangeIcons);
		}

		private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (Form childForm in MdiChildren)
			{
				childForm.Close();
			}
		}

		public void HighlightStackPointer(int address)
		{
			foreach(var mt in _memoryTools)
				mt.Highlight(address);
		}
		public void RefreshAddress(Dictionary<int, byte> addressValueMap)
		{
			foreach (var mt in _memoryTools)
			{
				mt.RefreshAddress(addressValueMap);
				foreach (var v in _objectViewers)
					v.RefreshAddress(addressValueMap);
			}
		}

		public void CreateViewer(string viewerName, ObjectItem[] objectItem)
		{
			var viewer = new ObjectViewer(viewerName, objectItem);
			_objectViewers.Add(viewer);
			viewer.Show(dp_Main, DockState.DockLeft);
		}

		public void AddViewerType(TypeItem type)
		{
			ObjectViewer.AddType(type);
		}
		// Hz = op per second
		// fr = 1000msec
		// n = ?msec
		public void RefreshProcessorStatus(long ticks, int frequency)
		{
			
			lbl_Ticks.Text = ticks.ToString("G");
			lbl_Frequency.Text = string.Format("{0}MHz",frequency/1000000);
			lbl_Elapsed.Text = TimeSpan.FromTicks((long)(TimeSpan.TicksPerMillisecond * (ticks * 1000d / frequency))).ToString(@"hh\:mm\:ss\.ffffff") + "μs";
		}

		public void RemoveBreakpoint(int line)
		{
			_asmContent.RemoveBreakpointMarker(line);
		}
		public void SetBreakpoint(int line)
		{
			_asmContent.SetBreakpointMarker(line);
		}
		public void JumpToLine(int line)
		{
			_asmContent.JumpToLine(line);
		}
		public void LoadAsmContent(LoadContentArgs args)
		{
			_asmContent.Load(args);
			foreach(var mt in _memoryTools)
				mt.Load(args.Processor);
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			_emulatorPresenter.Step();
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			_emulatorPresenter.Run();
		}

		private void tsb_Reset_Click(object sender, EventArgs e)
		{
			_emulatorPresenter.Reset();
		}
	}
}
