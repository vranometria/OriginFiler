using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OriginFiler.Models
{
    public class Favarite:ICloneable
    {
        [DataMember]
        public string Label { get; set; } = "";

        [DataMember]
        public string Path { get; set; } = "";

        public object Clone()
        {
            return Util.DeepCopy(this);
        }

        override public string ToString()
        {
            return Label;
        }
    }
}
