using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tronics.DataConverter.UDF
{
    public class Trsum : BinaryFormatter
    {
        public Int16 testnum=0;	/* corresponds to Tstdr.testnum */
        public Int32 execcnt=0;	/* total number of test executions */
        public Int32 failcnt=0;	/* total number of test failures */
        public Int32 failcnt2=0;	/* total number of test failures (2) */
        [SerializeLength(4)]
        public string future = "";	/* future (set to null if not used) */
    }
}