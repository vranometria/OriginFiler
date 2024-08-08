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
    /// HotkeyWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class HotkeyWindow : Window
    {
        public Hotkey Hotkey { get; set; } = new Hotkey();

        public bool IsApply { get; set; }

        private HotkeyHelper? HotkeyHelper;

        public HotkeyWindow()
        {
            InitializeComponent();
        }

        public HotkeyWindow(Hotkey? hotkey, HotkeyHelper? hotkeyHelper) : this() 
        {
            if (hotkey != null)
            {
                KeyTextBox.Text = hotkey.Key.ToString();
                KeyTextBox.Tag = hotkey.Key;

                Ctrl.IsChecked = hotkey.Ctrl;
                Alt.IsChecked = hotkey.Alt;
                Shift.IsChecked = hotkey.Shift;
            }

            HotkeyHelper = hotkeyHelper;
        }

        private void KeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //キー入力がa-z以外の場合は処理を行わない
            if (e.Key < Key.A || e.Key > Key.Z)
            {
                return;
            }

            KeyTextBox.Text = e.Key.ToString();
            KeyTextBox.Tag = e.Key;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if(HotkeyHelper == null) { return; }

            Key key = (Key)KeyTextBox.Tag;
            Hotkey hotkey = new Hotkey { Key = key, Alt = Alt.IsChecked == true, Ctrl = Ctrl.IsChecked == true, Shift = Shift.IsChecked == true  };

            bool success = HotkeyHelper.Register(hotkey.GetModifireKeys(), key);

            if (success)
            {
                MessageBox.Show("Hotkey is registered successfully.");
                IsApply = true;
                Hotkey = hotkey;
                Close();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
