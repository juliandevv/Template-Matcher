using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_Matcher
{
    class MatcherSettings
    {
        public double Threshold1;
        public double Threshold2;
        public int ScaleFactor;
        public TemplateMatchingType SimilarityMeasure;

        public MatcherSettings()
        {
            Threshold1 = 0.85;
            Threshold2 = 0.85;
            ScaleFactor = 24;
            SimilarityMeasure = TemplateMatchingType.CcorrNormed;
        }

        public MatcherSettings(double threshold1, double threshold2)
        {
            Threshold1 = threshold1;
            Threshold2 = threshold2;
            ScaleFactor = 24;
            SimilarityMeasure = TemplateMatchingType.CcorrNormed;
        }

        public MatcherSettings(double threshold1, double threshold2, int sclaeFactor, TemplateMatchingType similarityMeasure)
        {
            Threshold1 = threshold1;
            Threshold2 = threshold2;
            ScaleFactor = sclaeFactor;
            SimilarityMeasure = similarityMeasure;
        }
    }
}
