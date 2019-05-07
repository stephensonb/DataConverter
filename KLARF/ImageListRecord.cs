using System;

namespace DataConverter.KLARF
{
    public class ImageListRecord
    {
        public Int32 Count;
        public Int32 ImageNumber;
        public Int32 ImageType;

        public override string ToString()
        {
            return String.Format("{0} {1} {2}\n\r", Count, ImageNumber, ImageType);
        }
    }

}
