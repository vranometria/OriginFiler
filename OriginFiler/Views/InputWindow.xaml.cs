using OriginFiler.Models;
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
using System.Windows.Shapes;

namespace OriginFiler.Views
{
    /// <summary>
    /// InputBox.xaml の相互作用ロジック
    /// </summary>
    public partial class InputWindow : Window
    {
        public HierarchyInfo Get => new HierarchyInfo { 
            Name = NameInputBox.Text.Trim(),
            FolderPath = FolderPathInputBox.Text.Trim()
        };

        public InputWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameInputBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                NameInputBox.Background = Brushes.Red;
                return;
            }

            DialogResult = true;
        }
    }
}
