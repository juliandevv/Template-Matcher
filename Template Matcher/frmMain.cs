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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnSelectSearchImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                var fileString = Openfile.FileName;
                Mat img = new Mat(fileString);
                if (img != null)
                {
                    imbSearchImage.Image = img;
                }
            }
        }

        private void btnSelectTemplateImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                var fileString = Openfile.FileName;
                Mat img = new Mat(fileString);
                if (img != null)
                {
                    imbTemplateImage.Image = img;
                }
            }
        }

        private void btnViewPyramid_Click(object sender, EventArgs e)
        {
            var pyramidView = new frmViewPyramid(imbSearchImage.Image as Mat);
            pyramidView.Show();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            imbSearchImage.Image.Dispose();
            imbTemplateImage.Image.Dispose();
        }

        private void btnProcessImage_Click(object sender, EventArgs e)
        {
            Matcher.ChangeSettings(new MatcherSettings(0.85, 0.85, 12, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed));
            Matcher.MatchTemplate(imbSearchImage.Image as Mat, imbTemplateImage.Image as Mat, out var result);
            //var result = CrossCorrelation.ComputeCC(imbSearchImage.Image as Mat, imbTemplateImage.Image as Mat);

            if (result != null)
            {
                imbProcessedImage.Image = result;
            }

            //try
            //{
                
            //}
            //catch (ArgumentNullException ex)
            //{
            //    MessageBox.Show("Error", "Cannot process image. Check input images and try again", MessageBoxButtons.OK);
            //}
        }
    }
}
