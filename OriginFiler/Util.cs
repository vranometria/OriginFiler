using OriginFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Windows.Controls;

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
    }
}
