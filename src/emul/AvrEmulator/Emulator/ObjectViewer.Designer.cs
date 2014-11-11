namespace Emulator
{
	partial class ObjectViewer
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
			this.olv_Objects = new BrightIdeasSoftware.TreeListView();
			this.olvName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvValue = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvDec = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvBin = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			((System.ComponentModel.ISupportInitialize)(this.olv_Objects)).BeginInit();
			this.SuspendLayout();
			// 
			// olv_Objects
			// 
			this.olv_Objects.AllColumns.Add(this.olvName);
			this.olv_Objects.AllColumns.Add(this.olvValue);
			this.olv_Objects.AllColumns.Add(this.olvDec);
			this.olv_Objects.AllColumns.Add(this.olvBin);
			this.olv_Objects.AllColumns.Add(this.olvType);
			this.olv_Objects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvName,
            this.olvValue,
            this.olvDec,
            this.olvBin,
            this.olvType});
			this.olv_Objects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.olv_Objects.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.olv_Objects.Location = new System.Drawing.Point(0, 0);
			this.olv_Objects.Name = "olv_Objects";
			this.olv_Objects.OwnerDraw = true;
			this.olv_Objects.ShowGroups = false;
			this.olv_Objects.Size = new System.Drawing.Size(756, 410);
			this.olv_Objects.TabIndex = 0;
			this.olv_Objects.UseCompatibleStateImageBehavior = false;
			this.olv_Objects.View = System.Windows.Forms.View.Details;
			this.olv_Objects.VirtualMode = true;
			// 
			// olvName
			// 
			this.olvName.AspectName = "Name";
			this.olvName.Text = "Name";
			this.olvName.Width = 171;
			// 
			// olvValue
			// 
			this.olvValue.AspectName = "Value";
			this.olvValue.Text = "Value";
			this.olvValue.Width = 107;
			// 
			// olvDec
			// 
			this.olvDec.AspectName = "Dec";
			this.olvDec.Text = "Decimal";
			this.olvDec.Width = 85;
			// 
			// olvBin
			// 
			this.olvBin.AspectName = "Bin";
			this.olvBin.Text = "Binary";
			this.olvBin.Width = 93;
			// 
			// olvType
			// 
			this.olvType.AspectName = "Type";
			this.olvType.Text = "Type";
			// 
			// ObjectViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(756, 410);
			this.Controls.Add(this.olv_Objects);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Name = "ObjectViewer";
			this.Text = "ObjectViewer";
			((System.ComponentModel.ISupportInitialize)(this.olv_Objects)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private BrightIdeasSoftware.TreeListView olv_Objects;
		private BrightIdeasSoftware.OLVColumn olvName;
		private BrightIdeasSoftware.OLVColumn olvValue;
		private BrightIdeasSoftware.OLVColumn olvDec;
		private BrightIdeasSoftware.OLVColumn olvBin;
		private BrightIdeasSoftware.OLVColumn olvType;

	}
}