using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.IO;

namespace OriginFiler.Views
{
    /// <summary>
    /// TabContent.xaml の相互作用ロジック
    /// </summary>
    public partial class TabContent : UserControl
    {
        public TabContent(string folderPath)
        {
            InitializeComponent();
            OpenFolder(folderPath);
            ChangeFolder(folderPath);
        }

        private void ChangeFolder(string folderPath)
        {
            FolderPathTextBox.Text = folderPath;
            OpenFolder(folderPath);
        }

        /// <summary>
        /// 指定したフォルダの中身を表示する
        /// </summary>
        /// <param name="folderPath"></param>
        private void OpenFolder(string folderPath)
        {
            ObjectListView.Items.Clear();

            string[] files = Directory.GetFiles(folderPath);
            string[] directories = Directory.GetDirectories(folderPath);
            foreach (string file in files.Concat(directories))
            {
                var view = new ObjectView(file);
                view.MouseDoubleClick += delegate (object sender, MouseButtonEventArgs e)
                {
                    if (sender is ObjectView objectView)
                    {
                        try 
                        {
                            if (Directory.Exists(objectView.ObjectPath))
                            {
                                ChangeFolder(objectView.ObjectPath);
                            }
                            else if (File.Exists(objectView.ObjectPath))
                            {
                                Process.Start(objectView.ObjectPath);
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                };
                ObjectListView.Items.Add(view);
            }
        }
    }
}
