using System;

namespace DataConverter.KLARF
{
    public class InspectedArea
    {
        public Single XOffset;
        public Single YOffset;
        public Single XSize;
        public Single YSize;
        public Int32 XRepeat;
        public Int32 YRepeat;
        public Single XPitch;
        public Single YPitch;

        public override string ToString()
        {
            return String.Format("{0:f3} {1:f3} {2:f3} {3:f3} {4:f3} {5} {6:f3} {7:f3}\n\r", XOffset, YOffset, XSize, YSize, XRepeat, YRepeat, XPitch, YPitch);
        }

    }

}
