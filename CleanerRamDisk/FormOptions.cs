using CleanerRamDisk.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            if (Settings.Default.FileReserved == null)
                Settings.Default.FileReserved = new StringCollection();
		    foreach (var file in Settings.Default.FileReserved)
		    {
		        listReserv.Items.Add(file);
		    }
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Settings.Default.CleanPath = textBox1.Text;
			if (Settings.Default.MinSize == 0 && Settings.Default.MinSize >= 100)
				numericUpDown1.Value = 3;
			Settings.Default.MinSize = (byte)numericUpDown1.Value;
			Settings.Default.TimerClean = (byte)numericUpDown2.Value;
            Settings.Default.FileReserved.Clear();
		    foreach (var item in listReserv.Items)
		    {
		        Settings.Default.FileReserved.Add(item.ToString());
		    }
			Settings.Default.Save();
			Close();
		}

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowReadOnly = true;
            dialog.ReadOnlyChecked = true;
            dialog.CheckFileExists = false;
            dialog.ValidateNames = false;
            dialog.InitialDirectory = Settings.Default.CleanPath;
            if (dialog.ShowDialog() == DialogResult.OK && !listReserv.Items.Contains(dialog.FileName))
            {
                listReserv.Items.Add(dialog.FileName);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (listReserv.SelectedIndex < 0)
                return;
            listReserv.Items.Remove(listReserv.SelectedItem);
        }
    }
}
