using System;

namespace DataConverter.KLARF
{
    public class ResultTimestampRecord
    {
        public string Date;
        public string Time;

        public override string ToString()
        {
            return String.Format("{0} {1};\n\r", Date, Time);
        }
    }

}
