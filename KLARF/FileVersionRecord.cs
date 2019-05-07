using System;

namespace DataConverter.KLARF
{
    public class FileVersionRecord
    {
        public Int32 Major=0;
        public Int32 Minor=0;

        public override string ToString()
        {
            return String.Format("FileVersion {0} {1};\n\r", Major, Minor);
        }

    }

}
