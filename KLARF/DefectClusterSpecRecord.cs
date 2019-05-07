using System;
using System.Collections.Generic;

namespace DataConverter.KLARF
{
    public class DefectClusterSpecRecord
    {
        public Int32 Count;
        public string[] ClusterSpecNames;
        public string[] ClusterSpecValues;

        public DefectClusterSpecRecord(string count)
        {
            Count = Int32.Parse(count);
            ClusterSpecNames = new string[Count];
            ClusterSpecValues = new string[Count];
        }
        
        public Dictionary<string, Type> ClusterSpecValueTypes = new Dictionary<string, Type>() 
        {
            {"THRESHOLD", typeof(Single)},
            {"MINSIZE", typeof(Int16)},
            {"MERGTOL", typeof(Int32)}
        };

        public override string ToString()
        {
 	        string s=string.Format("DefectClusterSpec {0} ", Count);
            for(int i=0;i<Count;i++)
            {
                s = s + ClusterSpecNames[i] + " ";
            }
            s=s+"\n\rDefectClusterSetup ";
            for(int i=0;i<Count;i++)
            {
                s = s + ClusterSpecValues[i] + " ";
            }
            s=s.Trim()+";\n\r";
            return s;
        }
    }

}
