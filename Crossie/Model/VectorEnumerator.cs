using Crossie.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Crossie.Model
{
    class VectorEnumerator <T> : IEnumerable <T>
    {
        Vector<T> vectorList;

        public VectorEnumerator(Vector<T> vectorList)
        {
            this.vectorList = vectorList;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((ICollection<T>)vectorList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<T>)vectorList).GetEnumerator();
        }
    }
}
