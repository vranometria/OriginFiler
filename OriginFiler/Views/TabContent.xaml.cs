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
using OriginFiler.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OriginFiler.Views
{
    /// <summary>
    /// TabContent.xaml の相互作用ロジック
    /// </summary>
    public partial class TabContent : UserControl
    {
        private string FolderPath { get; set; }

        private string HomeFolderPath { get; set; }

        private ObservableCollection<ListViewData> ListViewDatas { get; set; } = new();

        public TabContent(string folderPath)
        {
            InitializeComponent();
            HomeFolderPath = folderPath;
            FolderPath = folderPath;
            ChangeFolder(folderPath);
        }

        private void ChangeFolder(string folderPath)
        {
            DirectoryBreadcrumb.ChangeDirectory(folderPath);
            OpenFolder(folderPath);
        }

        private void ChangeFolderByBreadcrumb(string folderPath)
        {
            OpenFolder(folderPath);
        }

        /// <summary>
        /// 指定したフォルダの中身を表示する
        /// </summary>
        /// <param name="folderPath"></param>
        private void OpenFolder(string folderPath)
        {
            ObservableCollection<ListViewData> listViewDatas = [];

            try
            {
                string[] files = Directory.GetFiles(folderPath);
                string[] directories = Directory.GetDirectories(folderPath);
                foreach (string file in files.Concat(directories))
                {
                    var view = new ObjectView(file);
                    view.MouseDoubleClick += delegate (object sender, MouseButtonEventArgs e)
                    {
                        if (sender is ObjectView objectView)
                        {
                            if (Directory.Exists(objectView.ObjectPath))
                            {
                                ChangeFolder(objectView.ObjectPath);
                            }
                            else if (File.Exists(objectView.ObjectPath))
                            {
                                Util.OpenFile(objectView.ObjectPath);
                            }
                        }
                    };
                    listViewDatas.Add(new ListViewData(view));
                };
                ListViewDatas = listViewDatas;
                ObjectListView.ItemsSource = ListViewDatas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Reload()    
        {
            DirectoryInfo? currentDir = new(FolderPath);
            if (currentDir.Exists)
            {
                ChangeFolder(currentDir.FullName);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DirectoryBreadcrumb.PathChanged += (s, p) => ChangeFolderByBreadcrumb(p);
        }

        /// <summary>
        /// ListViewヘッダークリックイベント ソートキーでソートする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader gridViewColumnHeader = (GridViewColumnHeader)sender;
            string? sortBy = gridViewColumnHeader.Tag.ToString();

            ICollectionView view = CollectionViewSource.GetDefaultView(ObjectListView.ItemsSource);
            ListSortDirection direction = view.SortDescriptions.FirstOrDefault().Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            SortDescription currentSort = new(sortBy, direction);

            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(currentSort);
            view.Refresh();
        }

        /// <summary>
        /// 上階層に移動するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpperDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            string? current = DirectoryBreadcrumb.DirectoryPath;
            if(current == null) { return; }

            DirectoryInfo? parentDir = Directory.GetParent(current);
            if (parentDir != null)
            {
                ChangeFolder(parentDir.FullName);
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            Reload();
        }

        private void OpenExploreMenuitem_Click(object sender, RoutedEventArgs e)
        {
            Util.OpenExplorer(FolderPath);
        }

        private void NameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObjectListView.ItemsSource = ListViewDatas.Where(data => data.ObjectName.Contains(NameFilterTextBox.Text));
        }

        private void NameFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                NameFilterTextBox.Text = "";
                e.Handled = true;
            }
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            try 
            {
                files.ToList().ForEach(file =>
                {
                    string fileName = Path.GetFileName(file);
                    string destPath = Path.Combine(FolderPath, fileName);
                    if (File.Exists(file))
                    { 
                        File.Move(file, destPath);
                    }
                    else if (Directory.Exists(file))
                    {
                        Directory.Move(file, destPath);
                    }
                });
                Reload();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeFolder(HomeFolderPath);
        }
    }
}
