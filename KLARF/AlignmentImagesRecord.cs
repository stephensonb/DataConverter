using System;
using System.Linq;
using System.Text;

namespace DataConverter.KLARF
{
    public class AlignmentImagesRecord
    {
        public Int32 Count;
        public AlignmentImage[] AlignmentImages;

        public AlignmentImagesRecord(Int32 points)
        {
            Count = points;
            AlignmentImages = new AlignmentImage[Count];
        }

        public void Add(Int32 markid, double x, double y, Int32 imagenumber)
        {
            int c = AlignmentImages.Count() + 1;

            AlignmentImages[c].X = x;
            AlignmentImages[c].Y = y;
            AlignmentImages[c].MarkID = markid;
            AlignmentImages[c].ImageNumber = imagenumber;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("AlignmentImages {0} \n\r", Count));
            foreach (AlignmentImage a in AlignmentImages)
            {
                s.Append(a.ToString());
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

}
