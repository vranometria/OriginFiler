using OriginFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OriginFiler.Models.Events;

namespace OriginFiler
{
    public class AppDataManager
    {
        private const string APP_DATA_FILE = "app.json";

        private static AppDataManager? instance;

        public static AppDataManager Instance
        {
            get
            {
                instance ??= new AppDataManager();
                return instance;
            }
        }

        private AppData AppData;

        /// <summary>
        /// お気に入りが追加されたときに発生します。
        /// </summary>
        public event EventHandler? FavariteChanged;

        public List<HierarchyInfo> Hierarchies => AppData.Hierarchies;

        public List<Favarite> Favarites 
        {
            get => AppData.Favarites;
            set 
            {
                AppData.Favarites = value;
                FavariteChanged?.Invoke(this, new FavariteAddedEventArgs(value.Last()));
            }
        }

        public Hotkey Hotkey
        {
            get => AppData.Hotkey;
            set => AppData.Hotkey = value;
        }

        private AppDataManager()
        {
            AppData? appData = null;
            if (File.Exists(APP_DATA_FILE))
            {
                appData = Util.ReadFile(APP_DATA_FILE);
            }

            AppData = appData ?? new AppData();
        }

        /// <summary>
        /// 保存する
        /// </summary>
        /// <param name="hierarchyInfos"></param>
        public void Save(List<HierarchyInfo> hierarchyInfos)
        {
            AppData.Hierarchies = hierarchyInfos;
            Util.WriteFile(APP_DATA_FILE, AppData);
        }

        internal void AddFavarite(Favarite favarite)
        {
            AppData.Favarites.Add(favarite);
            FavariteChanged?.Invoke(this, new FavariteAddedEventArgs(favarite));
        }

        internal bool IsRegisteredFavarite(string objectPath)
        {
            return AppData.Favarites.Any(f => f.Path == objectPath);
        }
    }
}
