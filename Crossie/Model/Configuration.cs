using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossie.Model
{
    class Configuration
    {
        public int GroupLimit { get; set; }
        public int PointsPerWord { get; set; }

        public Configuration() { }
        public Configuration(int GroupLimit, int PointsPerword)
        {
            this.GroupLimit = GroupLimit;
            this.PointsPerWord = PointsPerWord;
        }
    }
}
