namespace TTreeGui
{
	partial class TTreeForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.m_deleteButton = new System.Windows.Forms.Button();
			this.m_addButton = new System.Windows.Forms.Button();
			this.m_valueTextBox = new System.Windows.Forms.TextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel7 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.m_newPic = new System.Windows.Forms.PictureBox();
			this.panel8 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel5 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.m_oldPic = new System.Windows.Forms.PictureBox();
			this.panel6 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel7.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_newPic)).BeginInit();
			this.panel8.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_oldPic)).BeginInit();
			this.panel6.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add( this.m_deleteButton );
			this.panel1.Controls.Add( this.m_addButton );
			this.panel1.Controls.Add( this.m_valueTextBox );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point( 0, 0 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 884, 47 );
			this.panel1.TabIndex = 0;
			// 
			// m_deleteButton
			// 
			this.m_deleteButton.Location = new System.Drawing.Point( 330, 9 );
			this.m_deleteButton.Name = "m_deleteButton";
			this.m_deleteButton.Size = new System.Drawing.Size( 75, 23 );
			this.m_deleteButton.TabIndex = 2;
			this.m_deleteButton.Text = "Delete";
			this.m_deleteButton.UseVisualStyleBackColor = true;
			this.m_deleteButton.Click += new System.EventHandler( this.m_deleteButton_Click );
			// 
			// m_addButton
			// 
			this.m_addButton.Location = new System.Drawing.Point( 248, 9 );
			this.m_addButton.Name = "m_addButton";
			this.m_addButton.Size = new System.Drawing.Size( 75, 23 );
			this.m_addButton.TabIndex = 1;
			this.m_addButton.Text = "Add";
			this.m_addButton.UseVisualStyleBackColor = true;
			this.m_addButton.Click += new System.EventHandler( this.m_addButton_Click );
			// 
			// m_valueTextBox
			// 
			this.m_valueTextBox.Location = new System.Drawing.Point( 13, 13 );
			this.m_valueTextBox.Name = "m_valueTextBox";
			this.m_valueTextBox.Size = new System.Drawing.Size( 229, 20 );
			this.m_valueTextBox.TabIndex = 0;
			this.m_valueTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler( this.m_valueTextBox_KeyDown );
			// 
			// panel2
			// 
			this.panel2.Controls.Add( this.panel7 );
			this.panel2.Controls.Add( this.splitter1 );
			this.panel2.Controls.Add( this.panel5 );
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point( 0, 47 );
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size( 884, 517 );
			this.panel2.TabIndex = 1;
			// 
			// panel7
			// 
			this.panel7.Controls.Add( this.panel3 );
			this.panel7.Controls.Add( this.panel8 );
			this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel7.Location = new System.Drawing.Point( 277, 0 );
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size( 607, 517 );
			this.panel7.TabIndex = 4;
			// 
			// panel3
			// 
			this.panel3.AutoScroll = true;
			this.panel3.BackColor = System.Drawing.Color.White;
			this.panel3.Controls.Add( this.m_newPic );
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point( 0, 13 );
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size( 607, 504 );
			this.panel3.TabIndex = 0;
			// 
			// m_newPic
			// 
			this.m_newPic.Location = new System.Drawing.Point( 0, 0 );
			this.m_newPic.Name = "m_newPic";
			this.m_newPic.Size = new System.Drawing.Size( 100, 50 );
			this.m_newPic.TabIndex = 0;
			this.m_newPic.TabStop = false;
			// 
			// panel8
			// 
			this.panel8.Controls.Add( this.label2 );
			this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel8.Location = new System.Drawing.Point( 0, 0 );
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size( 607, 13 );
			this.panel8.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
			this.label2.Location = new System.Drawing.Point( 0, 0 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 607, 13 );
			this.label2.TabIndex = 1;
			this.label2.Text = "New";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point( 274, 0 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 3, 517 );
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// panel5
			// 
			this.panel5.Controls.Add( this.panel4 );
			this.panel5.Controls.Add( this.panel6 );
			this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel5.Location = new System.Drawing.Point( 0, 0 );
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size( 274, 517 );
			this.panel5.TabIndex = 2;
			// 
			// panel4
			// 
			this.panel4.AutoScroll = true;
			this.panel4.BackColor = System.Drawing.Color.White;
			this.panel4.Controls.Add( this.m_oldPic );
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point( 0, 13 );
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size( 274, 504 );
			this.panel4.TabIndex = 1;
			// 
			// m_oldPic
			// 
			this.m_oldPic.Location = new System.Drawing.Point( 0, 0 );
			this.m_oldPic.Name = "m_oldPic";
			this.m_oldPic.Size = new System.Drawing.Size( 100, 50 );
			this.m_oldPic.TabIndex = 0;
			this.m_oldPic.TabStop = false;
			// 
			// panel6
			// 
			this.panel6.Controls.Add( this.label1 );
			this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel6.Location = new System.Drawing.Point( 0, 0 );
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size( 274, 13 );
			this.panel6.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
			this.label1.Location = new System.Drawing.Point( 0, 0 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 274, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Old";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TTreeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 884, 564 );
			this.Controls.Add( this.panel2 );
			this.Controls.Add( this.panel1 );
			this.MinimumSize = new System.Drawing.Size( 300, 300 );
			this.Name = "TTreeForm";
			this.Text = "TTreeForm";
			this.panel1.ResumeLayout( false );
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout( false );
			this.panel7.ResumeLayout( false );
			this.panel3.ResumeLayout( false );
			((System.ComponentModel.ISupportInitialize)(this.m_newPic)).EndInit();
			this.panel8.ResumeLayout( false );
			this.panel5.ResumeLayout( false );
			this.panel4.ResumeLayout( false );
			((System.ComponentModel.ISupportInitialize)(this.m_oldPic)).EndInit();
			this.panel6.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button m_addButton;
		private System.Windows.Forms.TextBox m_valueTextBox;
		private System.Windows.Forms.PictureBox m_newPic;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.PictureBox m_oldPic;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button m_deleteButton;
	}
}