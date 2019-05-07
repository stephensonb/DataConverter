using System;

namespace DataConverter.KLARF
{
    public class DatabaseAlignmentMark
    {
        public Int32 MarkID;
        public double AlignmentImageOriginX;
        public double AlignmentImageOriginY;
        public double AlignmentPointX;
        public double AlignmentPointY;

        public override string ToString()
        {
            return String.Format("{0} {1:f3} {2:f3} {3:f3} {4:f3}\n\r", MarkID, AlignmentImageOriginX, AlignmentImageOriginY, AlignmentPointX, AlignmentPointY);
        }
    }

}
