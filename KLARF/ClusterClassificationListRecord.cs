using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter.KLARF
{
    public class ClusterClassificationListRecord
    {
        public Int32 Count;
        public List<ClusterClassificationRecord> ClusterClassificationList;

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(string.Format("ClusterClassificationList {0}\n\r", Count));

            foreach (ClusterClassificationRecord ccr in ClusterClassificationList)
            {
                s.Append(ccr.ToString());
            }

            return s.ToString().Trim() + ";\n\r";
        }
    }

}
