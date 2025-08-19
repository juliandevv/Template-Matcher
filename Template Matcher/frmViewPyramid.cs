using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;

namespace Template_Matcher
{
    public partial class frmViewPyramid : Form
    {
        private ImagePyramid pyr;
        private List<ImageBox> pyrLayers;

        private const int MAX_HEIGHT = 480;
        private const int MAX_WIDTH = 640;

        public frmViewPyramid(Mat img)
        {
            InitializeComponent();

            InitializePyramid(img);
        }

        private void InitializePyramid(Mat img)
        {
            pyr = new ImagePyramid(img, 2, 32);
            pyrLayers = new List<ImageBox>();

            for (int i = 0; i < pyr.Height; i++)
            {
                var imgBox = new ImageBox();
                var layer = pyr.GetLayer(i);
                imgBox.Image = layer;
                imgBox.Height = layer.Height;
                imgBox.Width = layer.Width;
                imgBox.SizeMode = PictureBoxSizeMode.CenterImage;
                imgBox.FunctionalMode = ImageBox.FunctionalModeOption.Minimum;
                pyrLayers.Add(imgBox);
            }
        }

        private void frmViewPyramid_FormClosing(object sender, FormClosingEventArgs e)
        {
            pyr.Dispose();
        }

        private void frmViewPyramid_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < pyr.Height; i++)
            {
                pyrLayers[i].Height = pyrLayers[i].Height > MAX_HEIGHT ? MAX_HEIGHT : pyrLayers[i].Height;
                pyrLayers[i].Width = pyrLayers[i].Width > MAX_HEIGHT ? MAX_WIDTH : pyrLayers[i].Width; ;
                this.Controls.Add(pyrLayers[i]);
                flpLayout.Controls.Add(pyrLayers[i]);
            }
        }
    }
}
