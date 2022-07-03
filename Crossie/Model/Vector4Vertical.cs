using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossie.Model
{
    class Vector4Vertical
    {
        public char Letter { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Orientation { get; set; }

        public Vector4Vertical() { }

        public Vector4Vertical(char Letter, int Row, int Column, string Orientation)
        {
            this.Letter = Letter;
            this.Row = Row;
            this.Column = Column;
            this.Orientation = Orientation;
        }
    }
}
