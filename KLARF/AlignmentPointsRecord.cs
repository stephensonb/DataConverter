using System;
using System.Linq;
using System.Text;

namespace DataConverter.KLARF
{
    public class AlignmentPointsRecord
    {
        public Int32 Count;
        public AlignmentPoint[] AlignmentPoints;

        public AlignmentPointsRecord(Int32 points)
        {
            Count = points;
            AlignmentPoints = new AlignmentPoint[Count];
        }

        public void Add(Int32 markid, double x, double y)
        {
            int c = AlignmentPoints.Count() + 1;

            AlignmentPoints[c].X = x;
            AlignmentPoints[c].Y = y;
            AlignmentPoints[c].MarkID = markid;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("AlignmentPoints {0} \n\r", Count));
            foreach(AlignmentPoint p in AlignmentPoints)
            {
                s.Append(p.ToString());
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

}
