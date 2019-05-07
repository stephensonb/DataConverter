using System;

namespace DataConverter.KLARF
{
    public class TiffSpecRecord
    {
        public string TiffVersion="";
        public string TiffAlignmentClass="";
        public string TiffImageClass="";

        public override string ToString()
        {
            return String.Format("TiffSpecRecord {0} {1} {2};\n\r", TiffVersion, TiffAlignmentClass, TiffImageClass);
        }
    }

}
