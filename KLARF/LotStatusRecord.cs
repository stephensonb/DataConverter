using System;

namespace DataConverter.KLARF
{
    public class LotStatusRecord
    {
        public Int32 WafersPassed;
        public Int32 WafersFailed;
        public Int32 WafersTested;

        public override string ToString()
        {
            return String.Format("LotStatus {0} {1} {2};\n\r", WafersPassed, WafersFailed, WafersTested);
        }
    }

}
