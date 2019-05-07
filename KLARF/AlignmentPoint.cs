using System;

namespace DataConverter.KLARF
{
    public class AlignmentPoint
    {
        public Int32 MarkID=0;
        public double X=0;
        public double Y=0;

        public override string ToString()
        {
            return String.Format("{0} {1:f3} {2:f3}\n\r", MarkID, X, Y);
        }

    }

}
