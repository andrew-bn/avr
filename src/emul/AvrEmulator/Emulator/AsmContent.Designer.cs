namespace Emulator
{
	partial class AsmContent
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AsmContent));
			this.rtb_Source = new FastColoredTextBoxNS.FastColoredTextBox();
			((System.ComponentModel.ISupportInitialize)(this.rtb_Source)).BeginInit();
			this.SuspendLayout();
			// 
			// rtb_Source
			// 
			this.rtb_Source.AutoCompleteBracketsList = new char[] {
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
			this.rtb_Source.AutoScrollMinSize = new System.Drawing.Size(37, 26);
			this.rtb_Source.BackBrush = null;
			this.rtb_Source.CharHeight = 26;
			this.rtb_Source.CharWidth = 13;
			this.rtb_Source.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.rtb_Source.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.rtb_Source.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb_Source.Font = new System.Drawing.Font("Consolas", 13.8F);
			this.rtb_Source.IsReplaceMode = false;
			this.rtb_Source.Location = new System.Drawing.Point(0, 0);
			this.rtb_Source.Name = "rtb_Source";
			this.rtb_Source.Paddings = new System.Windows.Forms.Padding(0);
			this.rtb_Source.ReadOnly = true;
			this.rtb_Source.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.rtb_Source.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("rtb_Source.ServiceColors")));
			this.rtb_Source.Size = new System.Drawing.Size(716, 432);
			this.rtb_Source.TabIndex = 0;
			this.rtb_Source.Zoom = 100;
			this.rtb_Source.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.rtb_Source_TextChanged);
			this.rtb_Source.PaintLine += new System.EventHandler<FastColoredTextBoxNS.PaintLineEventArgs>(this.rtb_Source_PaintLine);
			this.rtb_Source.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.rtb_Source_MouseDoubleClick);
			// 
			// AsmContent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(716, 432);
			this.Controls.Add(this.rtb_Source);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Name = "AsmContent";
			this.Text = "AsmContent";
			((System.ComponentModel.ISupportInitialize)(this.rtb_Source)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private FastColoredTextBoxNS.FastColoredTextBox rtb_Source;

	}
}