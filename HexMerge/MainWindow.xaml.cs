using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace HexMerge
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum Status
        {
            Idle,
            Ready,
            Success,
            Fail
        }

        private string BootloaderPath;
        private long BootloaderSize;
        private string AppPath;
        private long AppSize;

        public MainWindow()
        {
            InitializeComponent();
            UpdateStatus("Ready", Status.Idle);
        }

        private bool CheckFile()
        {
            if (BootloaderSize == 0) return false;
            if (AppSize == 0) return false;

            UpdateStatus("Files selected", Status.Ready);
            return true;
        }

        private bool MergeFiles(string target)
        {
            StreamWriter sw;
            string[] bl_file, app_file;

            try
            {
                sw = new StreamWriter(target);
            }
            catch(Exception e1)
            {
                UpdateStatus(e1.Message, Status.Fail);
                return false;
            }

            try
            {
                bl_file = File.ReadAllLines(BootloaderPath);
            }
            catch(Exception e2)
            {
                UpdateStatus(e2.Message, Status.Fail);
                return false;
            }

            try
            {
                for(long i = 0; i < bl_file.Length - 1; i++)
                    sw.WriteLine(bl_file[i]);
            }
            catch(Exception e3)
            {
                UpdateStatus(e3.Message, Status.Fail);
                return false;
            }

            try
            {
                app_file = File.ReadAllLines(AppPath);
            }
            catch(Exception e4)
            {
                UpdateStatus(e4.Message, Status.Fail);
                return false;
            }

            try
            {
                for (long i = 1; i < app_file.Length; i++)
                    sw.WriteLine(app_file[i]);
            }
            catch(Exception e5)
            {
                UpdateStatus(e5.Message, Status.Fail);
                return false;
            }

            sw.Close();
            return true;
        }

        private void UpdateStatus(string message, Status status)
        {
            statusLabel.Content = message;

            switch(status)
            {
                case Status.Idle:
                    statusBar.Background = Brushes.White;
                    statusLabel.Foreground = Brushes.Black;
                    break;
                case Status.Ready:
                    statusBar.Background = Brushes.Blue;
                    statusLabel.Foreground = Brushes.White;
                    break;
                case Status.Success:
                    statusBar.Background = Brushes.LightGreen;
                    statusLabel.Foreground = Brushes.Black;
                    break;
                case Status.Fail:
                    statusBar.Background = Brushes.OrangeRed;
                    statusLabel.Foreground = Brushes.White;
                    break;
            }
        }

        private void BootloaderPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Intel Hex file | *.hex";
            ofd.DefaultExt = ".hex";

            if(ofd.ShowDialog() == true)
            {
                BootloaderPath = ofd.FileName;
                BootloaderSize = new FileInfo(BootloaderPath).Length;

                blPath.Text = string.Format("{0} ({1} bytes)", Path.GetFileName(BootloaderPath), BootloaderSize);
            }

            mergeButton.IsEnabled = CheckFile();
        }

        private void AppPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Intel Hex file | *.hex";
            ofd.DefaultExt = ".hex";

            if (ofd.ShowDialog() == true)
            {
                AppPath = ofd.FileName;
                AppSize = new FileInfo(AppPath).Length;

                apPath.Text = string.Format("{0} ({1} bytes)", Path.GetFileName(AppPath), AppSize);
            }

            mergeButton.IsEnabled = CheckFile();
        }

        private void MergeButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Intel Hex file | *.hex";
            sfd.DefaultExt = ".hex";
            sfd.FileName = string.Format("Merged_{0}", DateTime.Now.ToString("yyyyMMdd"));

            if (sfd.ShowDialog() == true)
            {
                if (MergeFiles(sfd.FileName))
                    UpdateStatus(string.Format("{0} created successfully ({1} bytes)", Path.GetFileName(sfd.FileName), new FileInfo(sfd.FileName).Length), Status.Success);
                //else
                //    statusLabel.Content = string.Format("Error creating {0}", Path.GetFileName(sfd.FileName));
            }
        }
    }
}
