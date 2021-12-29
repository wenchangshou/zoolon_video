using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zoolon_webPlayer
{
    public partial class Form1 : Form
    {
        private const string V = "https://www.baidu.com/";

        public Form1()
        {

            InitializeComponent();
            webView21.Source = new Uri(V);
            
        }
    }
}
