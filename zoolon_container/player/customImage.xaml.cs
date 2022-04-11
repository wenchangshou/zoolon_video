using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace zoolon_container.player
{
    /// <summary>
    /// image.xaml 的交互逻辑
    /// </summary>
    public partial class customImage : UserControl
    {
        public customImage()
        {
            InitializeComponent();
        }
        [BindableAttribute(true)]
        public string Source
        {
            get { return (string)GetValue(TitlesProperty); }
            set { SetValue(TitlesProperty, value); this.image.Source = new BitmapImage(new Uri(value)); }
        }

        public static readonly DependencyProperty TitlesProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(customImage), new PropertyMetadata("custom image source"));
    }
}
