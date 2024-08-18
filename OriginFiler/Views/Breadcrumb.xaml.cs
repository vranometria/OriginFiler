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
        public string DirectoryPath {get; private set; } = "";

        /// <summary>
        /// パス変更イベント
        /// </summary>
        public event EventHandler<string>? PathChanged = null;

        public Breadcrumb(){ InitializeComponent(); }

        public Breadcrumb(string path): this()
        {
            DirectoryPath = path;
            DirectoryPathTextBox.Text = path;
        }

        public void ChangeDirectory(string path)
        {
            DirectoryPath = path;
            Show();
        }

        private void ChangeButtonMode()
        {
            DrawArea.Visibility = Visibility.Visible;
            DirectoryPathTextBox.Visibility = Visibility.Hidden;
        }

        private void ChangeTextMode() 
        {
            DrawArea.Visibility = Visibility.Hidden;
            DirectoryPathTextBox.Visibility = Visibility.Visible;
            DirectoryPathTextBox.Focus();
        }

        /// <summary>
        /// 現在パスにおける表示を更新する
        /// </summary>
        private void Show() 
        {
            if (string.IsNullOrEmpty(DirectoryPath)) { return; }

            DrawArea.Children.Clear();

            DirectoryPathTextBox.Text = DirectoryPath;

            string? p = DirectoryPath;
            List<Button> buttons = [];
            while (!string.IsNullOrEmpty(p))
            {
                string name = Path.GetFileName(p);
                if (string.IsNullOrEmpty(name)) 
                { 
                    DirectoryInfo directoryInfo = new(p);
                    name = directoryInfo.Root.Name;
                }

                Button button = new()
                {
                    Content = $" {name} ",
                    Tag = p,
                    Background = Brushes.White,
                    Width = double.NaN,
                    Height = double.NaN,
                    BorderThickness = new Thickness(0),
                };
                button.Click += (s, e) => {
                    string path = (s as Button)?.Tag as string ?? "";
                    DirectoryPath = path;
                    Show();
                    PathChanged?.Invoke(this, path);
                };

                buttons.Add(button);

                p = Path.GetDirectoryName(p);
            }

            buttons.Reverse();
            buttons.ForEach(button => {
                Label label = new Label
                {
                    Content = "▶",
                    Background = Brushes.White,
                    BorderThickness = new Thickness(0),
                    BorderBrush = Brushes.White,
                    Height = double.NaN,
                };
                DrawArea.Children.Add(button);
                DrawArea.Children.Add(label);
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Show();
        }

        private void DrawArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeTextMode();
        }

        private void DirectoryPathTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string path = DirectoryPathTextBox.Text;
                if (Directory.Exists(path) && PathChanged != null )
                {
                    DirectoryPath = path;
                    Show();
                    ChangeButtonMode(); 
                    PathChanged?.Invoke(this, path);
                }
            }
        }

        private void DirectoryPathTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeButtonMode();
        }
    }
}
