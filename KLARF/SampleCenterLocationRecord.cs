using System;

namespace DataConverter.KLARF
{
    public class SampleCenterLocationRecord
    {
        public double X=0;
        public double Y=0;

        public override string ToString()
        {
            return String.Format("SampleCenterLocation {0:f3} {1:f3};\n\r",X, Y);
        }

    }

}
