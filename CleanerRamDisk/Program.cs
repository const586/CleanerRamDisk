using CleanerRamDisk.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CleanerRamDisk
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new AppContext());
		}
	}

	public class AppContext : ApplicationContext
	{ // some comment
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
			out ulong lpFreeBytesAvailable,
			out ulong lpTotalNumberOfBytes,
			out ulong lpTotalNumberOfFreeBytes);
		private readonly NotifyIcon _ni;
		private const string PROGRAMM_NAME = "CleanerRAMDisk";
		private int count = 0;
        public AppContext()
		{
			_ni = new NotifyIcon {
				Text = PROGRAMM_NAME,
				Visible = true,
				Icon = Icon.FromHandle(Properties.Resources.broom_gray.GetHicon()),
			};
			_ni.DoubleClick += CleanClick;
			_ni.ContextMenuStrip = new ContextMenuStrip();
			_ni.ContextMenuStrip.Items.Add("Настройки", null, SettingsClick);
			_ni.ContextMenuStrip.Items.Add("Очистить", null, CleanClick);
			_ni.ContextMenuStrip.Items.Add("Выход", null, CloseClick);

			Timer t = new Timer();
			if (Settings.Default.TimerClean == 0)
				t.Interval = 65432;
			else
				t.Interval = Settings.Default.TimerClean * 1000;
			t.Tick += T_Tick;
			t.Start();
			T_Tick(null, null);
        }
        private void UpdateTitle(ulong free)
        {
            _ni.Text = $"{PROGRAMM_NAME}:{free}MiB:{count}";
        }
		public static bool DriveFreeBytes(string folderName, out ulong freespace, out ulong total)
		{
			freespace = 0;
			total = 0;
			if (string.IsNullOrEmpty(folderName))
			{
				throw new ArgumentNullException("folderName");
			}

			if (!folderName.EndsWith("\\"))
			{
				folderName += '\\';
			}

			ulong free = 0, dummy1 = 0, dummy2 = 0;

			if (GetDiskFreeSpaceEx(folderName, out free, out dummy1, out dummy2))
			{
				freespace = free;
				total = dummy1;
				return true;
			} else
			{
				return false;
			}
		}

		private void T_Tick(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(Settings.Default.CleanPath))
				return;
			DirectoryInfo info = new DirectoryInfo(Settings.Default.CleanPath);
			ulong free;
			ulong total;
			if (!DriveFreeBytes(info.FullName, out free, out total))
                return;
			ulong allowed = total * Settings.Default.MinSize / 100;
			if (allowed > free && allowed > 0)
				Clean(info);
            UpdateTitle((free / 1024 / 1024));
		}

		private void Clean(DirectoryInfo dir)
		{
			var files = dir.GetFiles("*", SearchOption.AllDirectories);
			foreach (var f in files)
			{
				try
				{
					f.Delete();
				} catch (Exception)
				{

				}
			}
			var dirs = dir.GetDirectories("*", SearchOption.AllDirectories);
			foreach (var d in dirs)
			{
				try
				{
					d.Delete();
				} catch (Exception)
				{
					
				}
			}
			count++;
		}

		private void CloseClick(object sender, EventArgs e)
		{
			_ni.Visible = false;
			ExitThread();
		}

		private void SettingsClick(object sender, EventArgs e)
		{
			new FormOptions().ShowDialog();
		}

		private void CleanClick(object sender, EventArgs e)
		{
			DirectoryInfo info = new DirectoryInfo(Settings.Default.CleanPath);
			Clean(info);
		}
	}
}
