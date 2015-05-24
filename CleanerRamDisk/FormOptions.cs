using CleanerRamDisk.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CleanerRamDisk
{
	public partial class FormOptions : Form
	{
		public FormOptions()
		{
			InitializeComponent();
			Icon = Icon.FromHandle(Resources.broom_gray.GetHicon());
			textBox1.Text = Settings.Default.CleanPath;
			if (Settings.Default.MinSize == 0 && Settings.Default.MinSize >= 100)
				numericUpDown1.Value = 3;
			else
				numericUpDown1.Value = Settings.Default.MinSize;
			if (Settings.Default.TimerClean == 0 && Settings.Default.MinSize >= 255)
				numericUpDown2.Value = 65;
			else
				numericUpDown2.Value = Settings.Default.TimerClean;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Settings.Default.CleanPath = textBox1.Text;
			if (Settings.Default.MinSize == 0 && Settings.Default.MinSize >= 100)
				numericUpDown1.Value = 3;
			Settings.Default.MinSize = (byte)numericUpDown1.Value;
			Settings.Default.TimerClean = (byte)numericUpDown2.Value;
			Settings.Default.Save();
			Close();
		}
	}
}
