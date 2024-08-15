using OriginFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Collections.Specialized;

namespace OriginFiler.Views
{
    /// <summary>
    /// ObjectView.xaml の相互作用ロジック
    /// </summary>
    public partial class ObjectView : UserControl
    {
        public ObjectInfo? ObjectInfo { get; private set; }

        public string? ObjectPath => ObjectInfo?.ObjectPath;

        public string ObjectName => $"{ObjectInfo?.ObjectName}";

        public ObjectView()
        {
            InitializeComponent();
        }

        public ObjectView(string objectPath):this()
        {
            ObjectInfo = new ObjectInfo(objectPath);
            ObjectNameLabel.Content = ObjectName;
            FileIconImage.Source = GetIconSource(objectPath);
        }

        private ImageSource GetIconSource(string objectPath) 
        {
            string extension = Path.GetExtension(objectPath).ToLower();
            string iconFileName = "object.png";
            switch (extension)
            {
                case ".xls":
                case ".xlsx":
                    iconFileName = "excel.png";
                    break;

                case ".doc":
                case ".docx":
                    iconFileName = "word.png";
                    break;

                case ".ppt":
                case ".pptx":
                    iconFileName = "powerpoint.png";
                    break;

                case ".txt":
                case ".json":
                case ".xml":
                case ".csv":
                    iconFileName = "text.png";
                    break;

                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".bmp":
                    iconFileName = "image.png";
                    break;
            }

            if (Directory.Exists(objectPath)) { iconFileName = "folder.png"; }

            string uri = $"pack://application:,,,/Images/Icons/{iconFileName}";
            return new BitmapImage(new Uri(uri));
        }

        /// <summary>
        /// コピーメニュークリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetFileDropList([ObjectPath]);
        }

        /// <summary>
        /// フルパスコピーメニュークリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyFullPathMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ObjectPath);
        }
    }
}
