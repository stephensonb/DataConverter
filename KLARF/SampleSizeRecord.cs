using System;

namespace DataConverter.KLARF
{
    public class SampleSizeRecord
    {
        public Int32 ShapeType=0;
        public Int32 Dimension1=0;
        public Int32 Dimension2=0;

        public override string ToString()
        {
            return String.Format("SampleSize {0} {1} {2};\n\r", ShapeType, Dimension1, Dimension2);
        }
    }

}
