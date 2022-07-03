using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossie.Model
{
    class SolutionSet
    {
        public string Orientation { get; set; }
        public int ColumnPosition { get; set; }
        public int RowPosition { get; set; }
        public string Name { get; set; }
        public SolutionSet()
        {
        }

        public SolutionSet (string Orientation, int RowPosition, 
                            int ColumnPosition, string Name)
        {
            this.Orientation = Orientation;
            this.RowPosition = RowPosition;
            this.ColumnPosition = ColumnPosition;
            this.Name = Name;
        }

    }

}
