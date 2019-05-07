using System.Collections.Generic;

namespace DataConverter.KLARF
{
    public class KLARFRecord
    {
        public string Tag;
        public List<string> DataFields;

        public KLARFRecord()
        {
            DataFields = new List<string>();
        }

        public string this[int index]
        {
            get
            {
                return DataFields[index];
            }
        }
    }

}
