using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFiler.Models.Events
{
    public class FavariteAddedEventArgs : EventArgs
    {
        public Favarite Favarite { get; }

        public FavariteAddedEventArgs(Favarite favarite)
        {
            Favarite = favarite;
        }
    }
}
