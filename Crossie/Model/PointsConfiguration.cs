using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossie.Model
{
    class PointsConfiguration
    {
        public string Orientation { get; set; }
        public char Letter { get; set; }
        public int Point { get; set; }

        public PointsConfiguration() { }
        public PointsConfiguration(string Orientation, char Letter, int Point)
        {
            this.Orientation = Orientation;
            this.Letter = Letter;
            this.Point = Point;
        }
    }
}
