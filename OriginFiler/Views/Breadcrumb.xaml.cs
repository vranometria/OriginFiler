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
    /// Breadcrumb.xaml の相互作用ロジック
    /// </summary>
    public partial class Breadcrumb : UserControl
    {
        /// <summary>
        /// 現在のパス
        /// </summary>
        public string? DirectoryPath { get; private set; }

        /// <summary>
        /// パス変更イベント
        /// </summary>
        public event EventHandler<string>? PathChanged = null;

        public Breadcrumb(){ InitializeComponent(); }

        public Breadcrumb(string path): this()
        {
            DirectoryPath = path;
        }

        private void Show() 
        {
            if (string.IsNullOrEmpty(DirectoryPath)) { return; }

            DrawArea.Children.Clear();

            List<Button> buttons = [];
            string? p = DirectoryPath;

            while (!string.IsNullOrEmpty(p))
            {
                Button buttton = new()
                {
                    Content = Path.GetDirectoryName(p),
                    Tag = p,
                    Background = Brushes.Transparent,
                };
                buttton.Click += (s, e) => {
                    PathChanged?.Invoke(this, p);
                };

                buttons.Add(p);

                // pの親ディレクトリを取得
                p = Path.GetDirectoryName(p);
            }

            buttons.Reverse();
            buttons.ForEach(button => { 
                DrawArea.Children.Add(button);
                DrawArea.Children.Add(new Label { Content = "▶" });
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Show();
        }

        private void DrawArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DrawArea.Visibility = Visibility.Hidden;
            DirectoryPathTextBox.Visibility = Visibility.Visible;
        }

        private void DirectoryPathTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            DrawArea.Visibility = Visibility.Visible;
            DirectoryPathTextBox.Visibility = Visibility.Hidden;
        }

        private void DirectoryPathTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string path = DirectoryPathTextBox.Text;
                if (Directory.Exists(path) && PathChanged != null )
                {
                    PathChanged?.Invoke(this, path);
                }
            }
        }
    }
}
