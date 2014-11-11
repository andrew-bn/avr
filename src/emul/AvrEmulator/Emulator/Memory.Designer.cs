using WeifenLuo.WinFormsUI.Docking;

namespace Emulator
{
	partial class Memory
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Memory));
			this.rtb_Memory = new FastColoredTextBoxNS.FastColoredTextBox();
			((System.ComponentModel.ISupportInitialize)(this.rtb_Memory)).BeginInit();
			this.SuspendLayout();
			// 
			// rtb_Memory
			// 
			this.rtb_Memory.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.rtb_Memory.AutoScrollMinSize = new System.Drawing.Size(386, 124);
			this.rtb_Memory.BackBrush = null;
			this.rtb_Memory.CharHeight = 31;
			this.rtb_Memory.CharWidth = 15;
			this.rtb_Memory.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.rtb_Memory.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.rtb_Memory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb_Memory.Font = new System.Drawing.Font("Consolas", 16.2F);
			this.rtb_Memory.IsReplaceMode = false;
			this.rtb_Memory.LineNumberFormat = "x4";
			this.rtb_Memory.LineNumberStartValue = ((uint)(0u));
			this.rtb_Memory.LineNumberStep = 16;
			this.rtb_Memory.Location = new System.Drawing.Point(0, 0);
			this.rtb_Memory.Name = "rtb_Memory";
			this.rtb_Memory.Paddings = new System.Windows.Forms.Padding(0);
			this.rtb_Memory.ReadOnly = true;
			this.rtb_Memory.ReservedCountOfLineNumberChars = 5;
			this.rtb_Memory.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.rtb_Memory.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("rtb_Memory.ServiceColors")));
			this.rtb_Memory.Size = new System.Drawing.Size(789, 551);
			this.rtb_Memory.TabIndex = 0;
			this.rtb_Memory.Text = "fastColoredTextBox1\r\nfdgsdf\r\ns\r\ndfgsfd";
			this.rtb_Memory.Zoom = 100;
			// 
			// Memory
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(789, 551);
			this.Controls.Add(this.rtb_Memory);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Name = "Memory";
			this.Text = "Memory";
			((System.ComponentModel.ISupportInitialize)(this.rtb_Memory)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private FastColoredTextBoxNS.FastColoredTextBox rtb_Memory;

	}
}
