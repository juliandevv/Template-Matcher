using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Template_Matcher
{
    public partial class ucFullSizeImage : UserControl
    {
        public ucFullSizeImage(Mat img)
        {
            InitializeComponent();

            this.Width = img.Width;
            this.Height = img.Height;
        }
    }
}
