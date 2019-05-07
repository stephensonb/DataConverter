using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter.KLARF
{
    public class SummarySpecRecord
    {
        public Int32 Count;
        public string[] Names;
        public List<string[]> SummarySpecList;

        public SummarySpecRecord(string count)
        {
            Count = Int32.Parse(count);
            Names = new string[Count];
        }

        public Dictionary<string, Type> SummarySpecTypes = new Dictionary<string, Type>() 
        {
            {"TESTNO", typeof(Int32)},
            {"NDEFECT", typeof(Int32)},
            {"DEFDENSITY", typeof(Single)},
            {"NDIE", typeof(Int32)},
            {"NDEFDIE", typeof(Int32)}
        };

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(string.Format("SummarySpec {0} ", Count));

            foreach (string n in Names)
            {
                s.Append(n + " ");
            }
            s.Append(";\n\rSummaryList \n\r");
            foreach (string[] summary in SummarySpecList)
            {
                for(int i=0;i<Count;i++)
                {
                    Type t = SummarySpecTypes[Names[i].ToUpper()];

                    if (t == typeof(Int32) || t == typeof(String) || t == typeof(Int16) || t == typeof(Byte) || t == typeof(Char))
                    {
                        s.Append(string.Format("{0} ", summary[i]));
                    } 
                    else
                    {
                        if (t == typeof(Single) || t == typeof(Double))
                        {
                            s.Append(string.Format("{0:f3} ", Double.Parse(summary[i])));
                        } 
                    }
                }
                s.Append("\n\r");
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

}
