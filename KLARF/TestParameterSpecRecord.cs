using System;
using System.Collections.Generic;

namespace DataConverter.KLARF
{
    public class TestParameterSpecRecord
    {
        public Int32 Count;
        public string[] TestParameterSpecNames;
        public string[] TestParameterSpecValues;

        public TestParameterSpecRecord(string count)
        {
            Count = Int32.Parse(count);
            TestParameterSpecNames = new string[Count];
            TestParameterSpecValues = new string[Count];
        }

        public Dictionary<string, Type> TestParameterSpecValueTypes = new Dictionary<string, Type>() 
        {
            {"PIXELSIZE", typeof(Single)},
            {"INSPECTIONMODE", typeof(Int16)},
            {"SAMPLEPERCENTAGE", typeof(Single)}
        };

        public override string ToString()
        {
            string s = string.Format("DefectClusterSpec {0} ", Count);
            for (int i = 0; i < Count; i++)
            {
                s = s + TestParameterSpecNames[i] + " ";
            }
            s = s + "\n\rDefectClusterSetup ";
            for (int i = 0; i < Count; i++)
            {
                Type t = TestParameterSpecValueTypes[TestParameterSpecNames[i].ToUpper()];

                if (t == typeof(Int32) || t == typeof(Int16) || t == typeof(String))
                {
                    s = s + TestParameterSpecValues[i] + " ";
                }
                else
                {
                    if (t == typeof(Single) || t == typeof(Double))
                    {
                        s = s + String.Format("{0:f3} ", Double.Parse(TestParameterSpecValues[i]));
                    }
                }
            }
            s = s.Trim() + ";\n\r";
            return s;
        }
    }

}
