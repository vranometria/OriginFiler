using OriginFiler.Models;
using OriginFiler.Views;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms.Integration;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Threading;

namespace OriginFiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDataManager AppDataManager = AppDataManager.Instance;

        /// <summary>ホットキーの操作クラス</summary>
        private HotkeyHelper? HotkeyHelper;

        /// <summary>ホットキーイベント</summary>
        private EventHandler? HotkeyEvent;

        private bool IsFirstOpen = true;

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ツリービューアイテムを作成する
        /// </summary>
        /// <param name="hierarchyInfo"></param>
        /// <returns></returns>
        private TreeViewItem CreateTreeItem(HierarchyInfo hierarchyInfo) 
        {
            TreeViewItem item = new()
            {
                Header = hierarchyInfo.Name,
                Tag = hierarchyInfo,
                Foreground = Brushes.White,
            };

            //ダブルクリックでフォルダを開く
            item.MouseDoubleClick += delegate(object sender, MouseButtonEventArgs e)
            {
                if (!IsFirstOpen) { return; }
                HierarchyInfo info = (HierarchyInfo)item.Tag;
                if (!string.IsNullOrEmpty(info.FolderPath))
                {
                    OpenTab(info);
                    IsFirstOpen = false;
                }
            };

            //右クリックメニュー(追加)
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

            //右クリックメニュー(開く)
            MenuItem openItem = new() { Header = "Open" };
            openItem.Click += delegate(object sender, RoutedEventArgs e)
            {
                HierarchyInfo info = (HierarchyInfo)item.Tag;
                if (!string.IsNullOrEmpty(info.FolderPath)){ OpenTab(info); }
            };

            //右クリックメニュー(削除)
            MenuItem deleteItem = new() { Header = "Delete" };
            deleteItem.Click += delegate(object sender, RoutedEventArgs e)
            {
                TreeViewItem? parentItem = item.Parent as TreeViewItem;
                if (parentItem != null)
                {
                    parentItem.Items.Remove(item);
                }
                else 
                {
                    TreeView parent = (TreeView) item.Parent;
                    parent.Items.Remove(item);
                }
            };

            ContextMenu contextMenu = new();
            contextMenu.Items.Add(addItem);
            contextMenu.Items.Add(openItem);
            contextMenu.Items.Add(Util.CreateOpenExploreMenuItem(hierarchyInfo.FolderPath));
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(deleteItem);
            item.ContextMenu = contextMenu;

            return item;
        }

        /// <summary>
        /// タブでフォルダの中身を表示する
        /// </summary>
        /// <param name="hierarchyInfo"></param>
        private void OpenTab(HierarchyInfo hierarchyInfo) 
        {
            TabItem tabItem = new()
            {
                Header = hierarchyInfo.Name,
                Content = new TabContent(hierarchyInfo.FolderPath),
            };
            tabItem.ContextMenu = Util.CreateTabContextMenu(Tab, tabItem);
            Tab.Items.Add(tabItem);
            Tab.SelectedItem = tabItem;
        }

        /// <summary>
        /// データを保存する
        /// </summary>
        private void Save() 
        {
            AppDataManager.Save(Util.CreateHierarchyInfos(FolderTreeView));
        }
        
        /// <summary>
        /// データを読み込む
        /// </summary>
        private void LoadData() 
        {
            AppDataManager.Hierarchies.ForEach( rootInfo => {
                TreeViewItem rootItem = CreateTreeItem(rootInfo);
                FolderTreeView.Items.Add(rootItem);
                BuildTreeView(rootInfo.Hierarchies, rootItem);
            });

            ListFavariteMenus();
        }

        /// <summary>
        /// ツリービューを表示
        /// </summary>
        /// <param name="childrenInfos"></param>
        /// <param name="parentItem"></param>
        private void BuildTreeView(List<HierarchyInfo> childrenInfos, TreeViewItem parentItem)
        {
            foreach (var childInfo in childrenInfos)
            {
                TreeViewItem childItem = CreateTreeItem(childInfo);
                parentItem.Items.Add(childItem);

                BuildTreeView(childInfo.Hierarchies, childItem);
            }
        }

        /// <summary>
        /// マウスポインタの位置に移動する
        /// </summary>
        private void MoveToMousePointerPosition()
        {
            if (GetCursorPos(out POINT point))
            {
                Left = point.X;
                Top = point.Y;
            }
        }

        /// <summary>
        /// お気に入りメニューを表示する
        /// </summary>
        private void ListFavariteMenus() 
        {
            FavariteMenu.Items.Clear();
            AppDataManager.Favarites.ForEach(favarite =>
            {
                MenuItem menuItem = new()
                {
                    Header = favarite.Label,
                    Background = (SolidColorBrush) Resources["BackGroundColor"],
                    Foreground = Brushes.White
                };
                menuItem.Click += delegate (object sender, RoutedEventArgs e)
                {
                    Util.Open(favarite.Path);
                };
                FavariteMenu.Items.Add(menuItem);
            });
        }

        /// <summary>
        /// 画面読み込みイベント
        /// ・設定の読み込み
        /// ・ホットキーの設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();

            AppDataManager.FavariteAdded += (sender, e) => { ListFavariteMenus(); };

            HotkeyEvent = (sender, e) =>
            {
                Hide();
                DispatcherTimer timer = new() { Interval = TimeSpan.FromMilliseconds(50) };
                timer.Tick += (sender, e) =>
                {
                    timer.Stop();
                    MoveToMousePointerPosition();
                    Show();
                    Activate();
                };
                timer.Start();
            };
            HotkeyHelper = new HotkeyHelper(this, HotkeyEvent);
            if (AppDataManager.Hotkey.IsValid) { HotkeyHelper.Register(AppDataManager.Hotkey.GetModifireKeys(), AppDataManager.Hotkey.Key); }
        }

        /// <summary>
        /// ツリービューの追加メニューイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewAddMenu_Click(object sender, RoutedEventArgs e)
        {
            InputWindow inputWindow = new InputWindow();
            bool? result = inputWindow.ShowDialog();
            if (result == true)
            {
                FolderTreeView.Items.Add(CreateTreeItem(inputWindow.Get));
            }
        }

        /// <summary>
        /// メイン画面マウスダウンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        /// <summary>
        /// 画面終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            Save();
            HotkeyHelper?.Dispose();
        }

        /// <summary>
        /// 閉じるボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// ホットキーメニューイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotkeyMenu_Click(object sender, RoutedEventArgs e)
        {
            HotkeyWindow hotkeyWindow = new HotkeyWindow(AppDataManager.Hotkey, HotkeyHelper);
            hotkeyWindow.ShowDialog();
            if(hotkeyWindow.IsApply)
            {
                AppDataManager.Hotkey = hotkeyWindow.Hotkey;
            }
        }

        /// <summary>
        /// メイン画面キーダウンイベント
        ///  ・ホットキーを設定している場合はescapeキーでウインドウを非表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && AppDataManager.Hotkey.IsValid )
            {
                Hide();
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsFirstOpen = true;
        }
    }
}