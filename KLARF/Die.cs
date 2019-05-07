using System;

namespace DataConverter.KLARF
{
    public class Die
    {
        public Int32 X;
        public Int32 Y;

        public override string ToString()
        {
            return string.Format("{0} {1}\n\r", X, Y);
        }
    }

}
