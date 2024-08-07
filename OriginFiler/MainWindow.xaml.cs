using OriginFiler.Models;
using OriginFiler.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace OriginFiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainViewModel mainViewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
        }

        private TreeViewItem CreateTreeItem(HierarchyInfo hierarchyInfo) 
        {
            TreeViewItem item = new TreeViewItem()
            {
                Header = hierarchyInfo.Name,
                Tag = hierarchyInfo,
                Foreground = Brushes.White,
            };
            item.MouseDoubleClick += delegate(object sender, MouseButtonEventArgs e)
            {
                HierarchyInfo info = (HierarchyInfo)item.Tag;
                if (!string.IsNullOrEmpty(info.FolderPath))
                {
                    OpenTab(info);
                }
            };
           

            MenuItem addItem = new() { Header = "Add" };
            addItem.Click += delegate(object sender, RoutedEventArgs e) 
            {
                InputWindow inputWindow = new InputWindow();
                bool? result = inputWindow.ShowDialog();
                if (result == true)
                {
                    item.Items.Add(CreateTreeItem(inputWindow.Get));
                }
            };

            MenuItem openItem = new() { Header = "Open" };
            openItem.Click += delegate(object sender, RoutedEventArgs e)
            {
                HierarchyInfo info = (HierarchyInfo)item.Tag;
                if (!string.IsNullOrEmpty(info.FolderPath)){ OpenTab(info); }
            };

            ContextMenu contextMenu = new();
            contextMenu.Items.Add(addItem);
            contextMenu.Items.Add(openItem);
            item.ContextMenu = contextMenu;

            return item;
        }

        private void OpenTab(HierarchyInfo hierarchyInfo) 
        {
            TabItem tabItem = new TabItem
            {
                Header = hierarchyInfo.Name,
                Content = new TabContent(hierarchyInfo.FolderPath),
            };
            Tab.Items.Add(tabItem);
            Tab.SelectedItem = tabItem;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            HierarchyInfo treeItem = (HierarchyInfo)menuItem.DataContext;
        }

        private void TreeViewAddMenu_Click(object sender, RoutedEventArgs e)
        {
            InputWindow inputWindow = new InputWindow();
            bool? result = inputWindow.ShowDialog();
            if (result == true)
            {
                FolderTreeView.Items.Add(CreateTreeItem(inputWindow.Get));
            }
        }

        private void TitleBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}