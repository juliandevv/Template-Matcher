using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_Matcher
{
    class CrossCorrelation
    {
        public static Mat GetFourierTransform(Mat img)
        {
            var result = new Mat();

            var dft = ComputeDFT(img);

            var mag = Magnitude(dft);
            mag = SwitchQuadrants(mag);

            CvInvoke.Normalize(mag, result, 0, 255, NormType.MinMax, DepthType.Cv8U);

            return result;
        }

        public static Mat ComputeCC(Mat img1, Mat img2)
        {
            var ccorr = new Mat();
            var result = new Mat();
            Mat zeroes;

            if (img1.Width >= img2.Width && img1.Height >= img2.Height)
            {
                //img1 = new Mat(img1, new Rectangle(Point.Empty, img2.Size));
                zeroes = new Mat(img1.Height, img1.Width, DepthType.Cv32F, img1.NumberOfChannels);
                var verticalPad = (img1.Height - img2.Height) / 2;
                var horizontalPad = (img1.Width - img2.Width) / 2;
                CvInvoke.CopyMakeBorder(img2, zeroes, verticalPad, verticalPad, horizontalPad, horizontalPad, BorderType.Constant, new Emgu.CV.Structure.MCvScalar(0, 0));
            }
            else
            {
                return null;
            }

            var fimg1 = ComputeDFT(img1);
            var fimg2 = ComputeDFT(zeroes);

            CvInvoke.MulSpectrums(fimg1, fimg2, ccorr, MulSpectrumsType.Default, true);

            CvInvoke.Dft(ccorr, result, DxtType.InvScale);

            var mag = Magnitude(ccorr);
            //mag = SwitchQuadrants(mag);

            CvInvoke.Normalize(mag, result, 0, 255, NormType.MinMax, DepthType.Cv8U);

            return result;
        }

        private static Mat ComputeDFT(Mat img)
        {
            var grayImg = new Mat();
            CvInvoke.CvtColor(img, grayImg, ColorConversion.Bgr2Gray);
            var m = CvInvoke.GetOptimalDFTSize(img.Rows);
            var n = CvInvoke.GetOptimalDFTSize(img.Cols);

            var bottom = m - img.Rows;
            var right = n - img.Cols;
            var padded = new Mat();

            CvInvoke.CopyMakeBorder(grayImg, padded, 0, bottom, 0, right, BorderType.Constant);
            padded.ConvertTo(padded, DepthType.Cv32F);

            Mat zeroMat = Mat.Zeros(padded.Rows, padded.Cols, DepthType.Cv32F, 1);
            VectorOfMat matVector = new VectorOfMat();
            matVector.Push(padded);
            matVector.Push(zeroMat);

            Mat complexI = new Mat(padded.Size, DepthType.Cv32F, 2);
            CvInvoke.Merge(matVector, complexI);

            var dft = new Mat(complexI.Size, DepthType.Cv32F, 2);

            CvInvoke.Dft(complexI, dft, DxtType.Forward, img.Rows);

            return dft;
        }

        private static Mat Magnitude(Mat fftData)
        {
            Mat real = new Mat(fftData.Size, DepthType.Cv32F, 1);

            Mat imaginary = new Mat(fftData.Size, DepthType.Cv32F, 1);
            VectorOfMat channels = new VectorOfMat();
            CvInvoke.Split(fftData, channels);

            real = channels.GetOutputArray().GetMat(0);
            imaginary = channels.GetOutputArray().GetMat(1);

            CvInvoke.Pow(real, 2.0, real);
            CvInvoke.Pow(imaginary, 2.0, imaginary);
            CvInvoke.Add(real, imaginary, real);
            CvInvoke.Pow(real, 0.5, real);

            Mat onesMat = Mat.Ones(real.Rows, real.Cols, DepthType.Cv32F, 1);
            CvInvoke.Add(real, onesMat, real);

            CvInvoke.Log(real, real);
            return real;
        }

        private static Mat SwitchQuadrants(Mat mat)
        {
            Mat result = mat;

            int cx = result.Cols / 2;
            int cy = result.Rows / 2;

            Mat q0 = new Mat(result, new Rectangle(0, 0, cx, cy));
            Mat q1 = new Mat(result, new Rectangle(cx, 0, cx, cy));
            Mat q2 = new Mat(result, new Rectangle(0, cy, cx, cy));
            Mat q3 = new Mat(result, new Rectangle(cx, cy, cx, cy));

            Mat temp = new Mat(q0.Size, DepthType.Cv32F, 1);

            q0.CopyTo(temp);
            q3.CopyTo(q0);
            temp.CopyTo(q3);

            q1.CopyTo(temp);
            q2.CopyTo(q1);
            temp.CopyTo(q2);

            return result;
        }

    }
}
