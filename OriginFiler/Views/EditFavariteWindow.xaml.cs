using OriginFiler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace OriginFiler.Views
{
    /// <summary>
    /// EditFavariteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class EditFavariteWindow : Window
    {
        private AppDataManager AppDataManager { get; set; } = AppDataManager.Instance;

        public List<Favarite> EditedFavarites 
        {
            get
            {
                List<Favarite> favarites = new();
                foreach (Label label in FavariteList.Items)
                {
                    favarites.Add((Favarite)label.Content);
                }
                return favarites;
            }
        }

        public EditFavariteWindow()
        {
            InitializeComponent();
        }

        private void Refresh()
        {
            int i = FavariteList.SelectedIndex;
            List<Favarite> items = new();
            foreach (Label label in FavariteList.Items)
            {
                Favarite favarite = (Favarite)label.Content;
                items.Add(favarite);
            }

            FavariteList.Items.Clear();
            items.ForEach(favarite =>
            {
                Favarite copy = (Favarite)favarite.Clone();
                Label label = new() { Content = copy };
                FavariteList.Items.Add(label);
            });
            FavariteList.SelectedIndex = i;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppDataManager.Favarites.ForEach(favarite =>
            {
                Favarite copy = (Favarite)favarite.Clone();
                Label label = new() { Content = copy };
                FavariteList.Items.Add(label);
            });

        }

        private void UpperButton_Click(object sender, RoutedEventArgs e)
        {
            if (FavariteList.SelectedIndex <= 0) return;

            object obj = FavariteList.SelectedItem;
            // 一つ上に移動
            int index = FavariteList.SelectedIndex;
            FavariteList.Items.RemoveAt(index);
            FavariteList.Items.Insert(index - 1, obj);
        }

        private void LowerButton_Click(object sender, RoutedEventArgs e)
        {
           if (FavariteList.SelectedIndex >= FavariteList.Items.Count - 1) return;

            object obj = FavariteList.SelectedItem;
            // 一つ下に移動
            int index = FavariteList.SelectedIndex;
            FavariteList.Items.RemoveAt(index);
            FavariteList.Items.Insert(index + 1, obj);
        }

        private void FavariteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FavariteList.SelectedIndex < 0) return;

            Favarite favarite = (Favarite)((Label)FavariteList.SelectedItem).Content;
            EditLabelTextBox.Text = favarite.Label;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if(FavariteList.SelectedIndex < 0) return;

            string s = EditLabelTextBox.Text;
            Favarite favarite = (Favarite)((Label)FavariteList.SelectedItem).Content;
            favarite.Label = s;
            Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (FavariteList.SelectedIndex < 0) return;

            FavariteList.Items.RemoveAt(FavariteList.SelectedIndex);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
