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
	public partial class MDIParent1 : Form, IEmulatorUI
	{
		private EmulatorPresenter _emulatorPresenter;
		private int childFormNumber = 0;
		private Memory _memoryTool;
		private AsmContent _asmContent;
		private List<ObjectViewer> _objectViewers = new List<ObjectViewer>();
		public MDIParent1()
		{
			InitializeComponent();
			_emulatorPresenter = new EmulatorPresenter(this);
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
			_memoryTool = new Memory();
			_memoryTool.Show(dp_Main);
			_asmContent = new AsmContent();
			_asmContent.Show(dp_Main);
		
			_emulatorPresenter.Load(fileName);
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

		public void RefreshAddress(Dictionary<int, byte> addressValueMap)
		{
			_memoryTool.RefreshAddress(addressValueMap);
			foreach(var v in _objectViewers)
				v.RefreshAddress(addressValueMap);
		}

		public void LoadView(string viewName, params ObjectItemAddress[] objectItems)
		{
			var viewer = new ObjectViewer(viewName, objectItems);
			_objectViewers.Add(viewer);
			viewer.Show(dp_Main);
		}

		public void JumpToLine(int line)
		{
			_asmContent.JumpToLine(line);
		}
		public void LoadAsmContent(LoadContentArgs args)
		{
			_asmContent.Load(args);
			_memoryTool.Load(args.Processor);
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			_emulatorPresenter.Step();
		}
	}
}
