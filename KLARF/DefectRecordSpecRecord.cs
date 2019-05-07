using System;
using System.Collections.Generic;

namespace DataConverter.KLARF
{
    public class DefectRecordSpecRecord
    {
        public Int32 Count;
        public string[] DefectRecordSpecNames;
        public List<string[]> DefectList;

        public DefectRecordSpecRecord(string count)
        {
            Count = Int32.Parse(count);
            DefectRecordSpecNames = new string[Count];
            DefectList = new List<string[]>();
        }

        public Dictionary<string, Type> DefectRecordSpecValueTypes = new Dictionary<string, Type>() 
        {
            {"DEFECTID", typeof(Int32)},
            {"X", typeof(Single)},
            {"Y", typeof(Single)},
            {"XREL", typeof(Single)},
            {"YREL", typeof(Single)},
            {"XINDEX", typeof(Int32)},
            {"YINDEX", typeof(Int32)},
            {"XSIZE", typeof(Single)},
            {"YSIZE", typeof(Single)},
            {"DEFECTAREA", typeof(Single)},
            {"DSIZE", typeof(Single)},
            {"CLASSNUMBER", typeof(Int32)},
            {"TEST", typeof(Int32)},
            {"IMAGECOUNT", typeof(Int32)},
            {"IMAGELIST", typeof(ImageListRecord)},
            {"CLUSTERNUMBER", typeof(Int32)},
            {"REVIEWSAMPLE", typeof(Int32)},
            {"ROUGHBINNUMBER", typeof(Int32)},
            {"FINEBINNUMBER", typeof(Int32)}
        };

        public override string ToString()
        {
            string s = string.Format("DefectRecordSpec {0} ", Count);
            for (int i = 0; i < Count; i++)
            {
                s = s + DefectRecordSpecNames[i] + " ";
            }
            s = s.Trim() + ";\n\rDefectList ";
            foreach(string[] d in DefectList)
            {
                for (int i = 0; i < Count; i++)
                {
                    Type t = DefectRecordSpecValueTypes[DefectRecordSpecNames[i].ToUpper()];

                    if (t == typeof(Int32) || t == typeof(Int16) || t == typeof(String))
                    {
                        s = s + d[i] + " ";
                    }
                    else
                    {
                        if (t == typeof(Single) || t == typeof(Double))
                        {
                            s = s + String.Format("{0:f3} ", Double.Parse(d[i]));
                        }
                    }
                }
                s = s.Trim() + "\n\r";
            }
            s = s.Trim() + ";\n\r";
            return s;
        }
    }

}
