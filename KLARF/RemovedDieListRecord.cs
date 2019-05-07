using System;
using System.Linq;
using System.Text;

namespace DataConverter.KLARF
{
    public class RemovedDieListRecord
    {
        public Int32 Count;
        public DieIndex[] RemovedDie;

        public RemovedDieListRecord(string count)
        {
            Count = Int32.Parse(count);
            RemovedDie = new DieIndex[Count];
        }

        public void Add(Int32 x, Int32 y)
        {
            int c = RemovedDie.Count() + 1;

            RemovedDie[c].X = x;
            RemovedDie[c].Y = y;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(string.Format("RemovedDieList {0}\n\r", Count));

            foreach (DieIndex di in RemovedDie)
            {
                s.Append(di.ToString());
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

}
