using System;

namespace DataConverter.KLARF
{
    public class ClusterClassificationRecord
    {
        public Int32 ClusterNumber;
        public Int32 Class;

        public override string ToString()
        {
            return string.Format("{0} {1}\n\r", ClusterNumber, Class);
        }
    }

}
