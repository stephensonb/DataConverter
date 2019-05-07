using System;
using System.Linq;
using System.Text;

namespace DataConverter.KLARF
{
    public class DatabaseAlignmentMarksRecord
    {
        public Int32 Count;
        public DatabaseAlignmentMark[] DatabaseAlignmentMarks;

        public DatabaseAlignmentMarksRecord(string count)
        {
            Count = Int32.Parse(count);
            DatabaseAlignmentMarks = new DatabaseAlignmentMark[Count];
        }

        public void Add(Int32 markid, double xorigin, double yorigin, double xpoint, double ypoint)
        {
            int c = DatabaseAlignmentMarks.Count() + 1;

            DatabaseAlignmentMarks[c].AlignmentImageOriginX = xorigin;
            DatabaseAlignmentMarks[c].AlignmentImageOriginY = yorigin;
            DatabaseAlignmentMarks[c].AlignmentPointX = xpoint;
            DatabaseAlignmentMarks[c].AlignmentPointY = ypoint;
            DatabaseAlignmentMarks[c].MarkID = markid;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            s.Append(String.Format("DatabaseAlignmentMarks {0}\n\r", Count));
            foreach (DatabaseAlignmentMark dba in DatabaseAlignmentMarks)
            {
                s.Append(dba.ToString());
            }
            return s.ToString().Trim() + ";\n\r";
        }
    }

}
