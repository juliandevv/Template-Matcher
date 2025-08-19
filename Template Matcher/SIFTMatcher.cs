using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;
using Emgu.CV.XFeatures2D;

namespace Template_Matcher
{
    class SIFTMatcher
    {
        private static void DetectAndComputeKeypointsSIFT(Mat input, out VectorOfKeyPoint keypoints, out Mat descriptors)
        {
            SIFT detector = new SIFT();
            keypoints = new VectorOfKeyPoint();
            descriptors = new Mat();
            detector.DetectRaw(input, keypoints);
            detector.Compute(input, keypoints, descriptors);

            //var rgbImage = input.Convert<Rgb, byte>();
            //Features2DToolbox.DrawKeypoints(input, keypoints, rgbImage, new Bgr(255, 255, 0));
        }

        public static void DetectAndComputeKeypointsFAST(Mat input, out VectorOfKeyPoint keypoints, out Mat descriptors)
        {
            FastFeatureDetector detector = new FastFeatureDetector(threshold: 20);
            BriefDescriptorExtractor descriptor = new BriefDescriptorExtractor(64);
            keypoints = new VectorOfKeyPoint();
            descriptors = new Mat();
            detector.DetectRaw(input, keypoints);
            descriptor.Compute(input, keypoints, descriptors);

            //var rgbImage = input.Convert<Rgb, byte>();
            //Features2DToolbox.DrawKeypoints(input, keypoints, rgbImage, new Bgr(255, 255, 0));
        }

        public static void EstimateAffine(Mat searchImage, Mat templateImage)
        {
            DetectAndComputeKeypointsSIFT(templateImage, out var templateKeypoints, out var templateDescriptors);
            DetectAndComputeKeypointsSIFT(searchImage, out var sceneKeypoints, out var sceneDescriptors);

            using (Mat searchKeyPts = new Mat())
            using (Mat templateKeyPts = new Mat())
            {
                Features2DToolbox.DrawKeypoints(searchImage, sceneKeypoints, searchKeyPts, new Bgr(0, 255, 0));
                CvInvoke.Imshow("Matches", searchKeyPts);
                CvInvoke.WaitKey();
            }

            VectorOfDMatch matches = new VectorOfDMatch();
            using (BFMatcher matcher = new BFMatcher(DistanceType.L2))
            {
                matcher.Match(sceneDescriptors, templateDescriptors, matches, null);
                
                var numMatches = matches.Size;
                PointF[] searchMatchedPoints = new PointF[numMatches];
                PointF[] templateMatchedPoints = new PointF[numMatches];

                for (int i = 0; i < numMatches; i++)
                {
                    searchMatchedPoints[i] = sceneKeypoints[matches[i].QueryIdx].Point;
                    templateMatchedPoints[i] = templateKeypoints[matches[i].TrainIdx].Point;
                }

                matches = new VectorOfDMatch(matches.ToArray().OrderByDescending(x => x.Distance).Take(30).ToArray());

                using (Mat matchImage = new Mat())
                {
                    Features2DToolbox.DrawMatches(templateImage, templateKeypoints, searchImage, sceneKeypoints, matches, matchImage, new MCvScalar(0, 255, 0), new MCvScalar(255, 0, 0));
                    CvInvoke.Imshow("Matches", matchImage);
                    CvInvoke.WaitKey();
                }

                using (Mat transform = CvInvoke.EstimateAffine2D(templateMatchedPoints, searchMatchedPoints, null, RobustEstimationAlgorithm.Ransac))
                {
                    if (transform.Cols == 3)
                    {
                        var lastRow = new Mat(1, 3, DepthType.Cv64F, 1);
                        lastRow.SetDoubleValue(0, 0, 0);
                        lastRow.SetDoubleValue(0, 1, 0);
                        lastRow.SetDoubleValue(0, 2, 1.0);
                        transform.PushBack(lastRow);

                        GetBestMatch(out var vtcs, templateImage.Size, transform);
                        CvInvoke.Polylines(searchImage, vtcs, true, new MCvScalar(0, 0, 255), 3);
                    }
                }
            }
            CvInvoke.Imshow("Matches", searchImage);
            CvInvoke.WaitKey();
        }
        
        public static void MatchFeaturesSIFT(Mat template, Mat scene)
        {
            //BFMatcher matcher = new BFMatcher(DistanceType.L2);
            //FlannBasedMatcher matcher = new FlannBasedMatcher(new Emgu.CV.Flann.LshIndexParams(6, 12, 1), new Emgu.CV.Flann.SearchParams(50));
            FlannBasedMatcher matcher = new FlannBasedMatcher(new Emgu.CV.Flann.KdTreeIndexParams(5), new Emgu.CV.Flann.SearchParams(50));

            DetectAndComputeKeypointsSIFT(template, out var templateKeypoints, out var templateDescriptors);
            DetectAndComputeKeypointsSIFT(scene, out var sceneKeypoints, out var sceneDescriptors);

            var matches = new VectorOfVectorOfDMatch();
            //matcher.Add();
            matcher.KnnMatch(sceneDescriptors, templateDescriptors, matches, 2);

            matches = RatioTestMatches(matches, 0.75);

            var sortedMatches = matches.ToArrayOfArray().OrderByDescending(x => x[0].Distance).Take(30).ToArray();
            matches = new VectorOfVectorOfDMatch(sortedMatches);

            Mat result = new Mat();
            Features2DToolbox.DrawMatches(template, templateKeypoints, scene, sceneKeypoints,
                    matches, result, new MCvScalar(0, 255, 255), new MCvScalar(255, 255, 255),
                    mask: null, flags: Features2DToolbox.KeypointDrawType.Default);

            var mask = new Mat();
            mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(255));

            //Features2DToolbox.VoteForSizeAndOrientation(templateKeypoints, sceneKeypoints,
            //matches, mask, 1.5, 20);

            var homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(templateKeypoints,
                           sceneKeypoints, matches, mask, 30);

            GetBestMatch(out var bestMatch, template.Size, homography);

            using (VectorOfPoint vp = new VectorOfPoint(bestMatch))
            {
                CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255), 5);
            }

            CvInvoke.Imshow("Matches", result);
            CvInvoke.WaitKey();
        }

        private static VectorOfVectorOfDMatch RatioTestMatches(VectorOfVectorOfDMatch matches, double ratio)
        {
            VectorOfVectorOfDMatch goodMatches = new VectorOfVectorOfDMatch();

            for (int i = 0; i < matches.Size; i++)
            {
                if (matches[i][0].Distance < ratio * matches[i][1].Distance)
                {
                    goodMatches.Push(matches[i]);
                }
            }

            return goodMatches;
        }

        private static void GetBestMatch(out Point[] vertices, Size templateSize, Mat homography)
        {
            Rectangle rect = new Rectangle(Point.Empty, templateSize);
            PointF[] pts = new PointF[]
            {
                    new PointF(rect.Left, rect.Bottom),
                    new PointF(rect.Right, rect.Bottom),
                    new PointF(rect.Right, rect.Top),
                    new PointF(rect.Left, rect.Top)
            };

            if (homography != null) pts = CvInvoke.PerspectiveTransform(pts, homography);

            vertices = Array.ConvertAll<PointF, Point>(pts, Point.Round);
        }
    }
}
