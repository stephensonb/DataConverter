using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter.KLARF
{
    public class ClassLookupRecord
    {
        public Int32 Count;
        public List<DefectClass> ClassList;

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(string.Format("ClassLookup {0}\n\r", Count));

            foreach (DefectClass dc in ClassList)
            {
                s.Append(dc.ToString());
            }

            return s.ToString().Trim() + ";\n\r";
        }
    }

}
