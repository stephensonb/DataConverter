using System;

namespace DataConverter.KLARF
{
    public class DieIndex
    {
        public Int32 X;
        public Int32 Y;

        public override string ToString()
        {
            return string.Format("{0} {1}\n\r", X, Y);
        }
    }

}
