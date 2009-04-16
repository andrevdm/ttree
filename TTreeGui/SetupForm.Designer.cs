namespace TTreeGui
{
	partial class SetupForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.m_graphvizPathTextBox = new System.Windows.Forms.TextBox();
			this.m_browseForDotButton = new System.Windows.Forms.Button();
			this.m_minItemsTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.m_maxItemsTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.m_startButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 13, 13 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 49, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Dot Path";
			// 
			// m_graphvizPathTextBox
			// 
			this.m_graphvizPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.m_graphvizPathTextBox.Location = new System.Drawing.Point( 69, 13 );
			this.m_graphvizPathTextBox.Name = "m_graphvizPathTextBox";
			this.m_graphvizPathTextBox.Size = new System.Drawing.Size( 315, 20 );
			this.m_graphvizPathTextBox.TabIndex = 1;
			
			// 
			// m_browseForDotButton
			// 
			this.m_browseForDotButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_browseForDotButton.Location = new System.Drawing.Point( 390, 13 );
			this.m_browseForDotButton.Name = "m_browseForDotButton";
			this.m_browseForDotButton.Size = new System.Drawing.Size( 25, 23 );
			this.m_browseForDotButton.TabIndex = 2;
			this.m_browseForDotButton.Text = "...";
			this.m_browseForDotButton.UseVisualStyleBackColor = true;
			// 
			// m_minItemsTextBox
			// 
			this.m_minItemsTextBox.Location = new System.Drawing.Point( 69, 39 );
			this.m_minItemsTextBox.Name = "m_minItemsTextBox";
			this.m_minItemsTextBox.Size = new System.Drawing.Size( 60, 20 );
			this.m_minItemsTextBox.TabIndex = 4;
			this.m_minItemsTextBox.Text = "3";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 13, 39 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 52, 13 );
			this.label2.TabIndex = 3;
			this.label2.Text = "Min Items";
			// 
			// m_maxItemsTextBox
			// 
			this.m_maxItemsTextBox.Location = new System.Drawing.Point( 69, 65 );
			this.m_maxItemsTextBox.Name = "m_maxItemsTextBox";
			this.m_maxItemsTextBox.Size = new System.Drawing.Size( 60, 20 );
			this.m_maxItemsTextBox.TabIndex = 6;
			this.m_maxItemsTextBox.Text = "3";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 13, 65 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 55, 13 );
			this.label3.TabIndex = 5;
			this.label3.Text = "Max Items";
			// 
			// m_startButton
			// 
			this.m_startButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.m_startButton.Location = new System.Drawing.Point( 175, 100 );
			this.m_startButton.Name = "m_startButton";
			this.m_startButton.Size = new System.Drawing.Size( 75, 23 );
			this.m_startButton.TabIndex = 7;
			this.m_startButton.Text = "Start";
			this.m_startButton.UseVisualStyleBackColor = true;
			this.m_startButton.Click += new System.EventHandler( this.m_startButton_Click );
			// 
			// SetupForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 424, 128 );
			this.Controls.Add( this.m_startButton );
			this.Controls.Add( this.m_maxItemsTextBox );
			this.Controls.Add( this.label3 );
			this.Controls.Add( this.m_minItemsTextBox );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.m_browseForDotButton );
			this.Controls.Add( this.m_graphvizPathTextBox );
			this.Controls.Add( this.label1 );
			this.MinimumSize = new System.Drawing.Size( 238, 164 );
			this.Name = "SetupForm";
			this.Text = "TTree Gui";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox m_graphvizPathTextBox;
		private System.Windows.Forms.Button m_browseForDotButton;
		private System.Windows.Forms.TextBox m_minItemsTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox m_maxItemsTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button m_startButton;
	}
}

