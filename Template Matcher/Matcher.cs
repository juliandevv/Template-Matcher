using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_Matcher
{
    internal static class Matcher
    {
        private static bool showDebug = false;

        public static MatcherSettings settings = new MatcherSettings();

        public static void ChangeSettings(MatcherSettings newSettings)
        {
            settings = newSettings;
        }

        public static void MatchTemplate(Mat searchImage, Mat templateImage, out Mat resultImage)
        {
            if (searchImage == null) { throw new ArgumentNullException("Search Image"); }
            if (templateImage == null) { throw new ArgumentNullException("Template Image"); }

            CvInvoke.Imwrite("../../examples/Template.jpg", templateImage);
            CvInvoke.Imwrite("../../examples/Search.jpg", searchImage);

            resultImage = searchImage.Clone();

            // create pyramids
            var searchImagePyramid = new ImagePyramid(searchImage, 2, settings.ScaleFactor);
            var templateImagePyramid = new ImagePyramid(templateImage, 2, settings.ScaleFactor);
            CvInvoke.Imwrite("../../examples/Pyramid.jpg", searchImagePyramid.GetLayer(1));

            var thetaRange = 360;
            //var peakThresh = 4; // number of stds from the mean to be considered

            List<BoundingBox> initialMatches = new List<BoundingBox>();
            List<List<BoundingBox>> clusteredMatches = new List<List<BoundingBox>>();
            List<BoundingBox> finalMatches = new List<BoundingBox>();

            for (int i = 0; i < thetaRange; i += 2)
            {
                var matches = RotateAndGetMatches(searchImagePyramid.GetLayer(1), templateImagePyramid.GetLayer(1), i, settings.Threshold1);
                if (matches != null)
                {
                    initialMatches.AddRange(matches);
                }
            }

            //foreach (var match in initialMatches)
            //{
            //    //CvInvoke.Circle(resultImage, bbox.Location, 3, new MCvScalar(255, 0, 0), 2);
            //    CvInvoke.Rectangle(resultImage, match.Bounds.ScaleRectangle(searchImagePyramid.ScaleFactor), new MCvScalar(0, 255, 0));
            //}

            clusteredMatches = ClusterMatches(initialMatches, templateImagePyramid.GetLayer(1).Width / 2);

            foreach (var matchCluster in clusteredMatches)
            {
                
                //RefineMatchSIFT(searchImage, templateImage, matchCluster, searchImagePyramid.ScaleFactor);
                finalMatches.Add(RefineMatch(searchImage, templateImage, matchCluster, searchImagePyramid.ScaleFactor, settings.Threshold2));
            }

            finalMatches.RemoveAll(x => x == null);

            foreach (var match in finalMatches)
            {
                var rotatedRect = new RotatedRect(match.GetCenter(), templateImage.Size, Convert.ToSingle(-match.Rotation));
                var pts = Array.ConvertAll(rotatedRect.GetVertices(), Point.Round);
                CvInvoke.Polylines(resultImage, pts, true, new MCvScalar(0, 255, 0), 2);
                CvInvoke.Imwrite("../../examples/Result.jpg", resultImage);
                //CvInvoke.Rectangle(resultImage, match.Bounds, new MCvScalar(0, 0, 255));
            }

            // tree traversal of rotations

            // coarse match in first layer
            // prune bad rotations

            // group matches by 2d displacement

            //
        }

        private static List<BoundingBox> RotateAndGetMatches(Mat searchImage, Mat templateImage, int angle, double threshold)
        {
            var matches = new List<BoundingBox>();
            var center = new PointF(templateImage.Width / 2f, templateImage.Height / 2f);
            var enclosingRect = new RotatedRect(center, templateImage.Size, angle);
            var boundingRect = enclosingRect.MinAreaRect();
            using (Mat rotationMatrix = new Mat())
            using (Mat rotatedTemplateImage = new Mat())
            using (Mat matchResult = new Mat())
            using (Mat filteredMatchResult = new Mat())
            {
                CvInvoke.GetRotationMatrix2D(center, angle, 1, rotationMatrix);
                var shiftX = rotationMatrix.GetDoubleValue(0, 2) + (boundingRect.Width / 2d - templateImage.Width / 2d);
                var shiftY = rotationMatrix.GetDoubleValue(1, 2) + (boundingRect.Height / 2d - templateImage.Height / 2d);
                rotationMatrix.SetDoubleValue(0, 2, shiftX);
                rotationMatrix.SetDoubleValue(1, 2, shiftY);

                CvInvoke.WarpAffine(templateImage, rotatedTemplateImage, rotationMatrix, boundingRect.Size, warpMethod: Warp.FillOutliers);

                CvInvoke.MatchTemplate(searchImage, rotatedTemplateImage, matchResult, settings.SimilarityMeasure);
                if (angle == 316)
                {
                    var savimg = new Mat();
                    matchResult.ConvertTo(savimg, DepthType.Cv8U, 255);
                    CvInvoke.Imwrite("../../examples/Rotate.jpg", rotatedTemplateImage);
                    CvInvoke.Imwrite("../../examples/InitialMatches.jpg", savimg);
                }

                double maxVal = 0;
                double minval = 0;
                var maxLoc = new Point();
                var minLoc = new Point();
                
                //var mean = new MCvScalar();
                //var std = new MCvScalar();
                //CvInvoke.MeanStdDev(matchResult, ref mean, ref std);
                //var threshold = mean.V0 + peakThresh * std.V0;
                //var threshold = maxVal - 1 * std.V0;
                //CvInvoke.Threshold(matchResult, filteredMatchResult, threshold, 255, ThresholdType.Binary);

                for (int i = 0; i < 5; i++)
                {
                    CvInvoke.MinMaxLoc(matchResult, ref minval, ref maxVal, ref minLoc, ref maxLoc);
                    CvInvoke.Circle(matchResult, maxLoc, 4, new MCvScalar(0, 0, 0), -1); // mask max location
                    
                    if (maxVal > threshold)
                    {
                        matches.Add(new BoundingBox(new Rectangle(maxLoc, boundingRect.Size), angle, maxVal));
                    }
                   
                    if (showDebug)
                    {
                        CvInvoke.Imshow("matches", matchResult);
                        CvInvoke.WaitKey(0);
                    }
                }

                if (showDebug)
                {
                    using (Mat drawImage = searchImage.Clone())
                    {
                        foreach (var match in matches)
                        {
                            CvInvoke.Circle(drawImage, Point.Round(match.Bounds.Location), 3, new MCvScalar(255, 0, 0), 2);
                            CvInvoke.Rectangle(drawImage, match.Bounds, new MCvScalar(0, 255, 0));
                        }

                        CvInvoke.Imshow("rotated", rotatedTemplateImage);
                        CvInvoke.Imshow("filtered matches", filteredMatchResult);
                        CvInvoke.Imshow("best matches", drawImage);
                        CvInvoke.WaitKey(500);
                    }
                }
            }

            return matches;
        }

        private static BoundingBox RefineMatch(Mat searchImage, Mat templateImage, List<BoundingBox> initialMatches, double scale, double threshold)
        {
            //var extremeTopLeft = initialMatches.OrderBy(x => Math.Sqrt(Math.Pow(x.Bounds.X, 2) + Math.Pow(x.Bounds.Y, 2))).FirstOrDefault().Bounds.Location;
            //var extremeBottomRight = initialMatches.OrderBy(x => Math.Sqrt(Math.Pow(x.Bounds.X, 2) + Math.Pow(x.Bounds.Y, 2))).LastOrDefault().Bounds.Location;

            var extremeTopLeft = new Point(
                initialMatches.OrderBy(x => x.Bounds.X).FirstOrDefault().Bounds.Location.X,
                initialMatches.OrderBy(x => x.Bounds.Y).FirstOrDefault().Bounds.Location.Y);

            var extremeBottomRight = new Point(
                initialMatches.OrderBy(x => x.Bounds.X).LastOrDefault().Bounds.Location.X,
                initialMatches.OrderBy(x => x.Bounds.Y).LastOrDefault().Bounds.Location.Y);

            var widthOffset = (extremeBottomRight.X - extremeTopLeft.X) * (int)scale;
            var heightOffset = (extremeBottomRight.Y - extremeTopLeft.Y) * (int)scale;
            var bestInitialMatch = initialMatches.OrderBy(x => x.Score).LastOrDefault();
            var matches = new List<BoundingBox>();
            double maxVal = 0;
            double minval = 0;
            var maxLoc = new Point();
            var minLoc = new Point();
            
            for (int i = -5; i <= 5; i++)
            {
                var angle = bestInitialMatch.Rotation + i * 0.4;
                if (angle < 0)
                {
                    angle = 0;
                }

                using (Mat rotatedTemplateImage = new Mat())
                {
                    RotateImage(templateImage, rotatedTemplateImage, angle, out var boundingRect);

                    var width = rotatedTemplateImage.Width + widthOffset;
                    var height = rotatedTemplateImage.Height + heightOffset;
                    var roi = new Rectangle(ScalePoint(extremeTopLeft, scale), new Size(width, height));

                    if (roi.X + roi.Width > searchImage.Width || roi.Y + roi.Height > searchImage.Height) return null;
                    if (roi.X < 0 || roi.Y < 0) { return null; }

                    using (Mat croppedSearchImage = new Mat(searchImage, roi))
                    using (Mat matchResult = new Mat())
                    {
                        CvInvoke.Imwrite("../../examples/ROI.jpg", croppedSearchImage);
                        CvInvoke.MatchTemplate(croppedSearchImage, rotatedTemplateImage, matchResult, settings.SimilarityMeasure);

                        CvInvoke.MinMaxLoc(matchResult, ref minval, ref maxVal, ref minLoc, ref maxLoc);
                        //CvInvoke.Imshow("rotatedImg", rotatedTemplateImage);
                        //CvInvoke.Imshow("searchImg", croppedSearchImage);
                        //CvInvoke.Imshow("match result", matchResult);
                        //CvInvoke.WaitKey(0);
                        
                        //var loc = new Point(Convert.ToInt32(maxLoc.X * scale), Convert.ToInt32(maxLoc.Y * scale));
                        maxLoc.Offset(ScalePoint(extremeTopLeft, scale));

                        if (maxVal > threshold)
                        {
                            matches.Add(new BoundingBox(new Rectangle(maxLoc, boundingRect.Size), angle, maxVal));
                        }
                    }
                }
            }

            var bestMatch = matches.OrderBy(x => x.Score).LastOrDefault();

            return bestMatch;
        }

        private static void RefineMatchSIFT(Mat searchImage, Mat templateImage, List<BoundingBox> initialMatches, double scale)
        {
            var extremeTopLeft = initialMatches.OrderBy(x => Math.Sqrt(Math.Pow(x.Bounds.X, 2) + Math.Pow(x.Bounds.Y, 2))).FirstOrDefault().Bounds.Location;
            var extremeBottomRight = initialMatches.OrderBy(x => Math.Sqrt(Math.Pow(x.Bounds.X, 2) + Math.Pow(x.Bounds.Y, 2))).LastOrDefault().Bounds.Location;
            var widthOffset = (extremeBottomRight.X - extremeTopLeft.X) * (int)scale;
            var heightOffset = (extremeBottomRight.Y - extremeTopLeft.Y) * (int)scale;
            var width = templateImage.Width + widthOffset;
            var height = templateImage.Height + heightOffset;
            var roi = new Rectangle(ScalePoint(extremeTopLeft, scale), new Size(width, height));

            using (Mat croppedSearchImage = new Mat(searchImage, roi))
            {
                SIFTMatcher.MatchFeaturesSIFT(templateImage, croppedSearchImage);
                //SIFTMatcher.EstimateAffine(croppedSearchImage, templateImage);
            }
        }

        private static List<List<BoundingBox>> ClusterMatches(List<BoundingBox> matches, double maxDist)
        {
            var clusters = new List<List<BoundingBox>>();
            
            for (int i = 0; i < matches.Count - 1; i++)
            {
                var cluster = new List<BoundingBox>();
                if (matches[i] != null)
                {
                    cluster.Add(matches[i]);
                    for (int j = i + 1; j < matches.Count; j++)
                    {
                        if (matches[j] != null)
                        {
                            var distanceBetweenPoints = Math.Sqrt(Math.Pow(matches[i].Bounds.X - matches[j].Bounds.X, 2) + Math.Pow(matches[i].Bounds.Y - matches[j].Bounds.Y, 2));
                            if (distanceBetweenPoints <= maxDist) //&& matches[i].Rotation >= matches[j].Rotation - 5 && matches[i].Rotation <= matches[j].Rotation + 5)
                            {
                                cluster.Add(matches[j]);
                                matches[j] = null;
                            }
                        }
                    }
                }

                if (cluster.Count > 0)
                {
                    clusters.Add(cluster.ConvertAll(x => new BoundingBox(x)));
                    cluster.Clear();
                }
            }

            return clusters;
        }

        private static void RotateImage(Mat input, Mat rotated, double angle, out Rectangle boundingRect)
        {
            var center = new PointF(input.Width / 2f, input.Height / 2f);
            var enclosingRect = new RotatedRect(center, input.Size, Convert.ToSingle(angle));
            boundingRect = enclosingRect.MinAreaRect();
            using (var rotationMatrix = new Mat())
            {
                CvInvoke.GetRotationMatrix2D(center, angle, 1, rotationMatrix);
                var shiftX = rotationMatrix.GetDoubleValue(0, 2) + (boundingRect.Width / 2d - input.Width / 2d);
                var shiftY = rotationMatrix.GetDoubleValue(1, 2) + (boundingRect.Height / 2d - input.Height / 2d);
                rotationMatrix.SetDoubleValue(0, 2, shiftX);
                rotationMatrix.SetDoubleValue(1, 2, shiftY);

                CvInvoke.WarpAffine(input, rotated, rotationMatrix, boundingRect.Size, warpMethod: Warp.FillOutliers);
            }
        }

        public static Rectangle ScaleRectangle(this Rectangle rect, int scale)
        {
            return new Rectangle(rect.X * scale, rect.Y * scale, rect.Width * scale, rect.Height * scale);
        }

        public static Point ScalePoint(this Point pt, double scale)
        {
            var x = Convert.ToInt32(pt.X * scale);
            var y = Convert.ToInt32(pt.Y * scale);
            return new Point(x, y);
        }

        private readonly struct Match
        {
            public readonly PointF Location;
            public readonly Rectangle BoundingBox;
            public readonly double Rotation;
            public readonly double Score;

            public Match(PointF location, Rectangle boundingBox, double rotation, double score)
            {
                this.Location = location;
                this.BoundingBox = boundingBox;
                this.Rotation = rotation;
                this.Score = score;
            }
        }

        private readonly struct MatchBox
        {
            public readonly PointF TopLeft;
            public readonly Size Size;
            public readonly int Rotation;
            public readonly double Score;

            public MatchBox(PointF topLeft, Size size, int rotation, double score)
            {
                this.TopLeft = topLeft;
                this.Size = size;
                this.Rotation = rotation;
                this.Score = score;
            }
        }

    }
}
