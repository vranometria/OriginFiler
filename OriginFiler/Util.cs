﻿using OriginFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.Json;
using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace OriginFiler
{
    public static class Util
    {
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
            //jsonをオブジェクトに変換する
            return JsonSerializer.Deserialize<AppData?>(json);
        }

        public static void SetHierarchies(TreeViewItem parentItem, HierarchyInfo parentHierarchyInfo)
        {   
            foreach (TreeViewItem childItem in parentItem.Items)
            {
                HierarchyInfo childInfo = (HierarchyInfo)childItem.Tag;
                parentHierarchyInfo.Hierarchies.Add(childInfo);
                SetHierarchies(childItem, childInfo);
            }
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
                hierarchyInfos.Add(model);
                SetHierarchies(rootItem, model);
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
    }
}
