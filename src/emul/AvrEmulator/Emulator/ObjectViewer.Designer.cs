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
			this.olvAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvValue = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			((System.ComponentModel.ISupportInitialize)(this.olv_Objects)).BeginInit();
			this.SuspendLayout();
			// 
			// olv_Objects
			// 
			this.olv_Objects.AllColumns.Add(this.olvName);
			this.olv_Objects.AllColumns.Add(this.olvAddress);
			this.olv_Objects.AllColumns.Add(this.olvValue);
			this.olv_Objects.AllColumns.Add(this.olvType);
			this.olv_Objects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvName,
            this.olvAddress,
            this.olvValue,
            this.olvType});
			this.olv_Objects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.olv_Objects.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.olv_Objects.Location = new System.Drawing.Point(0, 0);
			this.olv_Objects.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.olv_Objects.Name = "olv_Objects";
			this.olv_Objects.OwnerDraw = true;
			this.olv_Objects.ShowGroups = false;
			this.olv_Objects.Size = new System.Drawing.Size(1008, 505);
			this.olv_Objects.TabIndex = 0;
			this.olv_Objects.UseCompatibleStateImageBehavior = false;
			this.olv_Objects.View = System.Windows.Forms.View.Details;
			this.olv_Objects.VirtualMode = true;
			// 
			// olvName
			// 
			this.olvName.AspectName = "Name";
			this.olvName.DisplayIndex = 1;
			this.olvName.Text = "Name";
			this.olvName.Width = 171;
			// 
			// olvAddress
			// 
			this.olvAddress.AspectName = "Address";
			this.olvAddress.AspectToStringFormat = "[{0:x4}]";
			this.olvAddress.DisplayIndex = 0;
			this.olvAddress.Text = "";
			this.olvAddress.Width = 65;
			// 
			// olvValue
			// 
			this.olvValue.AspectName = "Value";
			this.olvValue.Text = "Value";
			this.olvValue.Width = 107;
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
			this.ClientSize = new System.Drawing.Size(1008, 505);
			this.Controls.Add(this.olv_Objects);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "ObjectViewer";
			this.Text = "ObjectViewer";
			((System.ComponentModel.ISupportInitialize)(this.olv_Objects)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private BrightIdeasSoftware.TreeListView olv_Objects;
		private BrightIdeasSoftware.OLVColumn olvName;
		private BrightIdeasSoftware.OLVColumn olvValue;
		private BrightIdeasSoftware.OLVColumn olvType;
		private BrightIdeasSoftware.OLVColumn olvAddress;

	}
}