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
using System.Windows.Navigation;
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
            FolderPathTextBox.Text = folderPath;
        }

        /// <summary>
        /// 指定したフォルダの中身を表示する
        /// </summary>
        /// <param name="folderPath"></param>
        private void OpenFolder(string folderPath)
        {
            ObjectListView.Items.Clear();

            // フォルダ内のファイルを取得
            string[] files = Directory.GetFiles(folderPath);
            string[] directories = Directory.GetDirectories(folderPath);
            foreach (string file in files.Concat(directories))
            {
                ObjectListView.Items.Add(new ObjectView(file));
            }
        }
    }
}
