using CleanerRamDisk.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BotInterface.Utils;
using BotInterface.Utils.Forms;

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
            string value = string.Empty;
            if (InputBox.ShowDialog("Имя файла(ов)", "Введите имя файла", ref value, ch => true, Description))
                listReserv.Items.Add(value);
        }

	    private string Description(string text)
	    {
	        if (string.IsNullOrEmpty(Settings.Default.CleanPath) || !Directory.Exists(Settings.Default.CleanPath))
	            return string.Empty;
	        var files = Directory.GetFiles(Settings.Default.CleanPath, text, SearchOption.AllDirectories);
	        return files.FirstOrDefault() ?? string.Empty;
	    }

	    private void btnDel_Click(object sender, EventArgs e)
        {
            if (listReserv.SelectedIndex < 0)
                return;
            listReserv.Items.Remove(listReserv.SelectedItem);
        }
    }
}
