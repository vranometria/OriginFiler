using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFiler.Models
{
    public class HierarchyInfo
    {
        public string Name { get; set; } = "";

        public string FolderPath { get; set; } = "";
        
        public List<HierarchyInfo> Hierarchies { get; set; } = new List<HierarchyInfo>();
    }
}
