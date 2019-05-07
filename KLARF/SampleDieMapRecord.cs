using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter.KLARF
{
    public class SampleDieMapRecord
    {
        public Int32 Count;
        public List<Die> DieList;

        public SampleDieMapRecord(string count)
        {
            Count = Int32.Parse(count);
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("SampleDieMap {0}\n\r", Count));

            foreach (Die d in DieList)
            {
                s.Append(d.ToString());
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

}
