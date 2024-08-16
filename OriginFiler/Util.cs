using OriginFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.Json;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Text.Json.Serialization;

namespace OriginFiler
{
    public static class Util
    {
        public static void Open(string path)
        {
            if (Directory.Exists(path))
            {
                OpenExplorer(path);
            }
            else if (File.Exists(path))
            {
                OpenFile(path);
            }
        }

        /// <summary>
        /// ファイルを実行する
        /// </summary>
        /// <param name="filePath"></param>
        public static void OpenFile(string filePath)
        {
            try 
            {
                ProcessStartInfo processStartInfo = new()
                {
                    FileName = filePath,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(filePath),
                };
                Process.Start(processStartInfo);
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// エクスプローラーでフォルダを開く
        /// </summary>
        /// <param name="folderPath"></param>
        public static void OpenExplorer(string folderPath)
        {
            Process.Start("EXPLORER.EXE", folderPath);
        }

        public static void WriteFile(string outputFilePath, AppData appData)
        {
            //jsonに変換する
            string json = JsonSerializer.Serialize(appData);
            //ファイルに書き込む
            File.WriteAllText(outputFilePath, json);
        }

        public static AppData? ReadFile(string inputFilePath)
        {
            //ファイルから読み込む
            string json = File.ReadAllText(inputFilePath);
            JsonSerializerOptions options = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };
            //jsonをオブジェクトに変換する
            return JsonSerializer.Deserialize<AppData?>(json, options);
        }

        public static List<HierarchyInfo> BuildHierarchies(TreeViewItem parentItem)
        {   
            List<HierarchyInfo> hierarchyInfos = [];

            foreach (TreeViewItem childItem in parentItem.Items)
            {
                HierarchyInfo childInfo = (HierarchyInfo)childItem.Tag;
                childInfo.Hierarchies = BuildHierarchies(childItem);
                hierarchyInfos.Add(childInfo);
            }
            return hierarchyInfos;
        }

        public static List<HierarchyInfo> CreateHierarchyInfos(TreeView treeView)
        {
            List<HierarchyInfo> hierarchyInfos = [];
            foreach (TreeViewItem rootItem in treeView.Items)
            {
                HierarchyInfo rootInfo = (HierarchyInfo)rootItem.Tag;
                HierarchyInfo model = new()
                {
                    Name = rootInfo.Name,
                    FolderPath = rootInfo.FolderPath,
                };
                model.Hierarchies = BuildHierarchies(rootItem);
                hierarchyInfos.Add(model);
            }
            return hierarchyInfos;
        }

        public static MenuItem CreateOpenExploreMenuItem(string folderPath)
        {
            MenuItem openExploreItem = new() { Header = "Open Explorer" };
            openExploreItem.Click += delegate (object sender, RoutedEventArgs e)
            {
                if (!string.IsNullOrEmpty(folderPath)) { Process.Start("EXPLORER.EXE", folderPath); }
            };
            return openExploreItem;
        }


        /// <summary>
        /// タブページのコンテキストメニューを作成する
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="tabItem"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ContextMenu CreateTabContextMenu(TabControl tab, TabItem tabItem)
        {
            //閉じるメニュー
            MenuItem closeItem = new() { Header = "Close Tab" };
            closeItem.Click += delegate (object sender, RoutedEventArgs e)
            {
                tab.Items.Remove(tabItem);
            };

            ContextMenu contextMenu = new();
            contextMenu.Items.Add(closeItem);
            return contextMenu;
        }

        public static void AddFavarite(string path)
        {
            AppDataManager appDataManager = AppDataManager.Instance;
            appDataManager.AddFavarite(new Favarite() { Label = Path.GetFileName(path), Path = path });
        }
    }
}
