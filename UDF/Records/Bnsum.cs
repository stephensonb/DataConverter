using System;

namespace DataConverter.UDF
{

    /// Bin summary record (Bnsum):
    ///
    ///	Each Bnsum indicates the number of times a device was placed in the
    /// specified bin category.

    public class Bnsum : BinaryFormatter
    {
        [SerializeLength(24)]
        public string binname = "";	/* bin name */
        public Int16 binnum=0;		/* bin number */
        public Int32 bincnt=0;		/* number of parts in bin */
        [SerializeLength(8)]
        public string future = "";	/* extra space for future expansion */
    }
}
