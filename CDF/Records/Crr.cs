using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tronics.DataConverter.CDF
{
    /// <summary>
    /// Crr Record Definition
    /// </summary>
    /// <remarks>
    /// Correlation Results record definition per CDF specification
    /// </remarks>
    public class Crr : BinaryFormatter
    {
        [SerializeLength(12)]
        public string partdesig = "";	/* part designation of correlation device */
        [SerializeLength(12)]
        public string sernum = "";	/* serial number of correlation device */
        public Int32 testdate=0;	/* time of correlation measurements */
        [SerializeLength(12)]
        public string testname = "";	/* correlation test name */
        public float idealval=0;	/* expected test value */
        public float actualval=0;	/* measured test value */
        public float deltamax=0;	/* max allowed value of (actual - ideal) */
        public float deltamin=0;	/* min allowed value of (actual - ideal) */
        [SerializeLength(16)]
        public string future = "";	/* extra space for future expansion */
    }
}
