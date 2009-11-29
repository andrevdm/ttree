using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TTree;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace TTreeGui
{
	public partial class TTreeForm : Form
	{
		private readonly string m_dotPath;
		private TTree<int> m_root;
		private string m_oldDot = null;

		public TTreeForm( string dotPath, int min, int max )
		{
			InitializeComponent();

			DeleteOldFiles();

			m_dotPath = dotPath;
			m_root = new TTree<int>( min, max );
			m_oldDot = m_root.ToDot();
			Redraw();
		}

		private void DeleteOldFiles()
		{
			DeleteOldFile( "old" );
			DeleteOldFile( "new" );
		}
		
		private void DeleteOldFile( string prefix )
		{
			string path = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
			string pngFile = Path.Combine( path, prefix + ".png" );

			if( File.Exists( pngFile ) )
				File.Delete( pngFile );
		}

		private void m_addButton_Click( object sender, EventArgs e )
		{
			AddValue();
		}

		private void m_valueTextBox_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Enter )
			{
				AddValue();
			}
		}

		private void m_deleteButton_Click( object sender, EventArgs e )
		{
			m_root.Remove( int.Parse( m_valueTextBox.Text ) );
			Redraw();

			ActiveControl = m_valueTextBox;
			m_valueTextBox.SelectAll();
		}

		private void AddValue()
		{
			if( !m_valueTextBox.Text.Contains( "," ) )
			{
				m_root.Add( int.Parse( m_valueTextBox.Text ) );
			}
			else
			{
				foreach( var s in m_valueTextBox.Text.Split( new[] { ',' } ) )
				{
					m_root.Add( int.Parse( s ) );
				}
			}

			Redraw();

			ActiveControl = m_valueTextBox;
			m_valueTextBox.SelectAll();
		}

		private void Redraw()
		{
			if( m_oldDot != null )
			{
				Redraw( m_oldDot, "old", m_oldPic );
			}

			m_oldDot = m_root.ToDot();
			Redraw( m_oldDot, "new", m_newPic );
		}

		private void Redraw( string dot, string prefix, PictureBox pbox )
		{
			if( pbox.Image != null )
			{
				pbox.Image.Dispose();
				pbox.Image = null;
			}

			string path = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
			string pngFile = Path.Combine( path, prefix + ".png" );

			var psi = new ProcessStartInfo();
			psi.FileName = m_dotPath;
			psi.Arguments = "-Tpng -o\"" + pngFile + "\"";
			psi.UseShellExecute = false;
			psi.RedirectStandardInput = true;
			psi.RedirectStandardOutput = false;
			psi.CreateNoWindow = true;
			psi.WindowStyle = ProcessWindowStyle.Hidden;

			using( var process = Process.Start( psi ) )
			{
				process.StandardInput.Write( dot );
				process.StandardInput.Close();
				process.WaitForExit();
			}

			pbox.Image = Image.FromFile( pngFile );
			pbox.Width = pbox.Image.Width;
			pbox.Height = pbox.Image.Height;
		}

		private void TTreeForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			if( m_newPic.Image != null )
				m_newPic.Image.Dispose();

			if( m_oldPic.Image != null )
				m_oldPic.Image.Dispose();
		}
	}
}
