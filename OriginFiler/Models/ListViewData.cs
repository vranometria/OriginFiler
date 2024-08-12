using OriginFiler.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFiler.Models
{
    public class ListViewData
    {
        public ObjectView ObjectView { get; set; }

        public FileTypes ObjectType { get; set; }

        public string ObjectName => ObjectView.ObjectName;

        public ListViewData(ObjectView objectView)
        {
            ObjectView = objectView;
            ObjectType = objectView.ObjectInfo == null ? FileTypes.None : objectView.ObjectInfo.FileType;
        }
    }
}
