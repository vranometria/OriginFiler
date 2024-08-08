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

        public static HierarchyInfo SetHierarchy(TreeViewItem item, HierarchyInfo hierarchyInfo)
        {
            foreach (TreeViewItem child in item.Items)
            {
                HierarchyInfo childInfo = (HierarchyInfo)child.Tag;
                hierarchyInfo.Hierarchies.Add(childInfo);
                SetHierarchy(child, childInfo);
            }
            return hierarchyInfo;
        }
    }
}
