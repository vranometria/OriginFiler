using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFiler.Models
{
    public class MainViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<HierarchyInfo> Items { get; set; } = new ObservableCollection<HierarchyInfo>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Add(HierarchyInfo treeItem) 
        {
            Items.Add(new HierarchyInfo());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
        }
    }
}
