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
    /// Tdc Record Definition
    /// </summary>
    /// <remarks>
    /// Test Decription record definition per CDF specification
    /// </remarks>
    public class Tdc : BinaryFormatter
    {
        [SerializeLength(12)]
        public string testname = "";	/* name of test */
        [SerializeLength(12)]
        public string seqname = "";	/* test category (magnetic, electrical, etc.) */
        [SerializeLength(12)]
        public string statname = "";	/* reserved for trend analysis use only */
        public Int16 testnum=0;	/* designation number for this test */
        [SerializeLength(6)]
        public string units = "";	/* units ofr limits and measurements */
        public float rangelo = 0;
        public float rangehi = 0;
        public float resolution = 0;
        public float lolimit=0;	/* minimum specification test limit */
        public float hilimit=0;	/* maximum specification test limit */
        public float anomfltrlo = 0;
        public float anomfltrhi = 0;
        public Int32 execcnt = 0;
        public Int32 failcnt = 0;
        public float minval = 0;
        public float maxval = 0;
        public float mean = 0;
        public float sigma = 0;
        public float skew = 0;
        public float kurt = 0;
        public Int16 anomlo = 0;
        public Int16 anomhi = 0;
        public Int16 cvrgcnt = 0;
        public Int32 statflg = 0;
        public Int32 failcnt2 = 0;
        [SerializeLength(2)]
        public string datatype = "";	/*  data type of stored test values */
        public Int16 datasizeb=0;	/* length of corresponding Trc.result records */
        public Int32 statcnt = 0;
        public float median = 0;
        public float uhng = 0;
        public float lhng = 0;
        public float uwsk = 0;
        public float lwsk = 0;
        public float uifnc = 0;
        public float lifnc = 0;
        public float uofnc = 0;
        public float lofnc = 0;
        public float rangelomax = 0;
        public float rangelomin = 0;
        [SerializeLength(12)]
        public string future = "";	/* future (set to null if not used) */
    }
}
