using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_Matcher
{
    class BoundingBox
    {
        private readonly Rectangle _rect;
        private readonly double _rotation;
        private readonly double _score;

        public Rectangle Bounds
        {
            get { return _rect; }
        }

        public double Rotation
        {
            get { return _rotation; }
        }

        public double Score
        {
            get { return _score; }
        }

        public BoundingBox(Rectangle rect, double rotation, double score)
        {
            _rect = rect;
            _rotation = rotation;
            _score = score;
        }

        public BoundingBox(BoundingBox bbox)
        {
            _rect = bbox.Bounds;
            _rotation = bbox.Rotation;
            _score = bbox.Score;
        }

        public PointF GetCenter()
        {
            return new PointF(_rect.Location.X + (_rect.Width / 2), _rect.Location.Y + (_rect.Height / 2));
        }
    }
}
