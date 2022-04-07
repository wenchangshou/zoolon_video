using LibVLCSharp.Shared;
using System;
using System.Windows;
namespace zoolon_container
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string WindowTitle = "山高月小";
        public static string ShowText { get { return "水落石出"; } }
        public MainWindow()
        {
            
            InitializeComponent();
            
        }
        public MainWindow(Options options)
        {
            InitializeComponent();
            Application.Current.MainWindow.Width = options.Width;
            Application.Current.MainWindow.Height = options.Height;
            Application.Current.MainWindow.Left = options.X;
            Application.Current.MainWindow.Top = options.Y;
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
        }
        public void initControl(Options options)
        {

        }
        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            Core.Initialize();
        }
    }
}
