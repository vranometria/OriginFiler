using OriginFiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace OriginFiler.Views
{
    /// <summary>
    /// ObjectView.xaml の相互作用ロジック
    /// </summary>
    public partial class ObjectView : UserControl
    {
        public ObjectInfo? ObjectInfo { get; private set; }

        public string? ObjectPath => ObjectInfo?.ObjectPath;

        public string ObjectName => $"{ObjectInfo?.ObjectName}";

        public ObjectView()
        {
            InitializeComponent();
        }

        public ObjectView(string objectPath):this()
        {
            ObjectInfo = new ObjectInfo(objectPath);
            ObjectNameLabel.Content = ObjectName;
        }
    }
}
