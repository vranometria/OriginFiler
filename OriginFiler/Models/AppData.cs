using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFiler.Models
{
    public class AppData
    {
        public List<HierarchyInfo> Hierarchies { get; set; } = [];

        public Hotkey Hotkey { get; set; } = new Hotkey();

        public List<Favarite> Favarites { get; set; } = [];
    }
}
