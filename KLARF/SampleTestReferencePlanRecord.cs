using System;
using System.Text;

namespace DataConverter.KLARF
{
    public class SampleTestReferencePlanRecord
    {
       public Int32 Count;
       public Die[] SampleDie;
       public Die[] CompareDie;

        public SampleTestReferencePlanRecord(string count)
        {
            Count = Int32.Parse(count);
            SampleDie = new Die[Count];
            CompareDie = new Die[Count];
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("SampleTestReferencePlan {0}\n\r", Count));

            for (int i = 0; i < Count; i++)
            {
                s.Append(SampleDie[i].ToString().Trim() + " " + CompareDie[i].ToString());
            }

            return s.ToString().Trim() + ";\n\r";
        }
    }

}
