using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFiler.Models
{
    public class ObjectInfo
    {
        public string ObjectName { get; set; }
        public string ObjectPath { get; set; }
        public FileTypes FileType { get; set; }

        public ObjectInfo(string filePath)
        {
            ObjectPath = filePath;
            ObjectName = Path.GetFileName(filePath);
            FileType = File.Exists(filePath) ? FileTypes.File : 
                Directory.Exists(filePath) ? FileTypes.Directory :
                FileTypes.None ;
        }
    }
}
