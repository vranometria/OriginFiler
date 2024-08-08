using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OriginFiler.Models
{
    public class Hotkey
    {
        public Key Key { get; set; }

        public bool Ctrl { get; set; }

        public bool Alt { get; set; }

        public bool Shift { get; set; }


        public bool IsValid => Key != Key.None;

        public ModifierKeys GetModifireKeys() 
        {
            ModifierKeys modifierKeys = ModifierKeys.None;
            if (Ctrl)
            {
                modifierKeys |= ModifierKeys.Control;
            }
            if (Alt)
            {
                modifierKeys |= ModifierKeys.Alt;
            }
            if (Shift)
            {
                modifierKeys |= ModifierKeys.Shift;
            }
            return modifierKeys;
        }
    }
}
