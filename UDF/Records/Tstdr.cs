using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tronics.DataConverter.UDF
{
    /*
	Test description record (Tstdr):
	One Tstdr is required for each type of test measurement to be taken.
    */
    public class Tstdr : BinaryFormatter
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
        public float lolimit=0;	/* minimum specification test limit */
        public float hilimit=0;	/* maximum specification test limit */
        public float fltrlo=0;		/* initial low limit for anomalie 'filter' */
        public float fltrhi=0;		/* initial high limit for anomalie 'fileter' */
        [SerializeLength(2)]
        public string datatype = "";	/*  data type of stored test values */
        public Int16 datasizeb=0;	/* length of corresponding Trc.result records */
        [SerializeLength(20)]
        public string future = "";	/* future (set to null if not used) */
    }
}
