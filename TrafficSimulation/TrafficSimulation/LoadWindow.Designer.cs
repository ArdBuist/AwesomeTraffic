namespace TrafficSimulation
{
	partial class LoadWindow
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
		public void InitializeComponent()
		{
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.progressBar2 = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 12);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(376, 20);
			this.progressBar1.TabIndex = 0;
			// 
			// progressBar2
			// 
			this.progressBar2.Location = new System.Drawing.Point(12, 48);
			this.progressBar2.Name = "progressBar2";
			this.progressBar2.Size = new System.Drawing.Size(376, 18);
			this.progressBar2.TabIndex = 2;
			// 
			// LoadWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(400, 78);
			this.Controls.Add(this.progressBar2);
			this.Controls.Add(this.progressBar1);
			this.Name = "LoadWindow";
			this.Text = "LoadWindow";
			this.ResumeLayout(false);
		}

		#endregion

		public System.Windows.Forms.ProgressBar progressBar1;
		public System.Windows.Forms.ProgressBar progressBar2;
	}
}