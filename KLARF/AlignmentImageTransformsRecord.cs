using System;
using System.Linq;
using System.Text;

namespace DataConverter.KLARF
{
    public class AlignmentImageTransformsRecord
    {
        public double[,] TransformMatrix = new double[2,2];
        public Int32 Count;
        public Int32[] AlignmentMarkIDs;

        public AlignmentImageTransformsRecord(Int32 count)
        {
            Count = count;
            AlignmentMarkIDs = new Int32[count];
        }

        public void Add(Int32 markid)
        {
            int c = AlignmentMarkIDs.Count() + 1;
            AlignmentMarkIDs[c] = markid;
        }

        public void SetTransform(double a11, double a12, double a21, double a22)
        {
            TransformMatrix[0, 0] = a11;
            TransformMatrix[0, 1] = a12;
            TransformMatrix[1, 0] = a21;
            TransformMatrix[1, 1] = a22;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("AlignmentImageTransforms {0:f3} {1:f3} {2:f3} {3:f3} {4}\n\r", TransformMatrix[0,0], TransformMatrix[0,1],TransformMatrix[1,0], TransformMatrix[1,1], Count));
            foreach (Int32 i in AlignmentMarkIDs)
            {
                s.Append(String.Format("{0}\n\r", i));
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

}
