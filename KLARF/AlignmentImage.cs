using System;

namespace DataConverter.KLARF
{
    public class AlignmentImage
    {
        public Int32 MarkID;
        public double X;
        public double Y;
        public Int32 ImageNumber;

        public override string ToString()
        {
            return String.Format("{0} {1:f3} {2:f3} {3}\n\r", MarkID, X, Y, ImageNumber);
        }
    }

}
