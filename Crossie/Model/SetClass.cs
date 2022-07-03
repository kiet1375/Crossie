using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossie.Model
{
    public class SetClass<T>
    {
        public Vector<T> Data { get; set; }

        public SetClass(Vector<T> d)
        {
            Data = d;
        }

        public bool Membership(T element)
        {
            return Data.Contains(element);
        }

        public bool IsSubsetOf(SetClass<T> B)
        {
            for (int i = 0; i < Data.Count; i++)
            {
                if (!(B.Data.Contains(Data[i])))
                    return false;
            }
            return true;
        }
        public bool IsSupersetOf(SetClass<T> B)
        {
            for (int i = 0; i < B.Data.Count; i++)
            {
                if (!(Data.Contains(B.Data[i])))
                    return false;
            }
            return true;
        }
        public SetClass<T> IntersectionWith(SetClass<T> B)
        {
            Vector<T> temp = new Vector<T>();

            for (int i = 0; i < B.Data.Count; i++)
            {
                if (Data.Contains(B.Data[i]))
                    temp.Add(B.Data[i]);
            }

            SetClass<T> result = new SetClass<T>(temp);

            return result;
        }
        public SetClass<T> UnionWith(SetClass<T> B)
        {
            Vector<T> temp = new Vector<T>();

            for (int i = 0; i < Data.Count; i++)
            {
                temp.Add(Data[i]);
                for (int j = 0; j < B.Data.Count; j++)
                {
                    if (!(Data.Contains(B.Data[j])))
                        if (!(temp.Contains(B.Data[j])))
                            temp.Add(B.Data[j]);
                }
            }
            SetClass<T> result = new SetClass<T>(temp);

            return result;
        }
        public SetClass<T> Difference(SetClass<T> B)
        {
            Vector<T> temp = new Vector<T>();

            for (int i = 0; i < Data.Count; i++)
            {
                if (!(B.Data.Contains(Data[i])))
                    temp.Add(B.Data[i]);
                
            }
            SetClass<T> result = new SetClass<T>(temp);

            return result;
        }
        public SetClass<T> Complement(SetClass<T> U)
        {
            Vector<T> temp = new Vector<T>();

            for (int i = 0; i < Data.Count; i++)
            {
                for (int j = 0; j < U.Data.Count; j++)
                {
                    if (!(Data.Contains(U.Data[j])))
                        if (!(temp.Contains(U.Data[j])))
                            temp.Add(U.Data[j]);
                }
            }
            SetClass<T> result = new SetClass<T>(temp);

            return result;
        }
        public SetClass<SetClass<T>> Powerset() { return null; }
        public SetClass<T> SymmetricDifference(SetClass<T> B)
        {
            Vector<T> temp = new Vector<T>();

            for (int i = 0; i < B.Data.Count; i++)
            {
                if (B.Data.Contains(Data[i]) == false)
                    temp.Add(Data[i]);
            }
            for (int i = 0; i < B.Data.Count; i++)
            {
                if (Data.Contains(B.Data[i]) == false)
                    temp.Add(B.Data[i]);
            }
            SetClass<T> result = new SetClass<T>(temp);
            return result;
        }
        public SetClass<Tuple<T, T2>> CartesianProduct<T2>(SetClass<T2> B) { return null; }


        public IEnumerator<T> GetEnumerator()
        {
            return ((ICollection<T>)Data).GetEnumerator();
        }

        public override string ToString()
        {
            string listValues = "";
            foreach (T t in this.Data)
            {
                listValues += t + ",";
            }
            return listValues;
        }

    }
}
