using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_Matcher
{
    public class ImagePyramid : IDisposable
    {
        private bool _isDisposed;

        private Mat[] layers;
        private int height;
        private int scaleFactor;

        public int Height
        {
            get { return height; }
        }

        public int ScaleFactor
        {
            get { return scaleFactor; }
        }

        public ImagePyramid(Mat img, int height, int scaleFactor)
        {
            this.scaleFactor = scaleFactor;

            int maxHeight = Convert.ToInt32(Math.Log(img.Width, scaleFactor));
            if (height > maxHeight) { this.height = maxHeight; }
            else { this.height = height; }

            layers = new Mat[this.height];
            layers[0] = img;

            for (int i = 1; i < height; i++)
            {
                layers[i] = new Mat();
                var targetSize = new Size(layers[i-1].Width / this.scaleFactor, layers[i-1].Height / this.scaleFactor);
                CvInvoke.Resize(layers[i - 1], layers[i], targetSize, interpolation: Inter.Area);
            }
        }

        public Mat GetLayer(int layer)
        {
            return layers[layer];
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    for (int i = 1; i < height; i++)
                    {
                        layers[i].Dispose();
                    }
                }
            }
        }
    }
}
