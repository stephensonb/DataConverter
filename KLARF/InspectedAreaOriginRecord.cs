using System;

namespace DataConverter.KLARF
{
    public class InspectedAreaOriginRecord
    {
        public Single X;
        public Single Y;

        public override string ToString()
        {
            return String.Format("{0} {1}\n\r", X, Y);
        }
    }

}
