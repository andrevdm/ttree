using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace TTreeGui
{
	public partial class SetupForm : Form
	{
		public SetupForm()
		{
			InitializeComponent();
			m_graphvizPathTextBox.Text = ConfigurationManager.AppSettings[ "dotPath" ];
		}

		private void m_startButton_Click( object sender, EventArgs e )
		{
			var frm = new TTreeForm( m_graphvizPathTextBox.Text, int.Parse( m_minItemsTextBox.Text ), int.Parse( m_maxItemsTextBox.Text ) );
			frm.ShowDialog();
		}
	}
}
