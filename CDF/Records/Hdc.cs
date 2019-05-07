using System;

namespace DataConverter.CDF
{
    /// <summary>
    /// Hdc Record Definition
    /// </summary>
    /// <remarks>
    /// Header record definition per CDF specification
    /// </remarks>

    public class Hdc : BinaryFormatter
    {
        [SerializeLength(6)]
        public string cdfver = "";

        [SerializeLength(12)]
        public string operators = "";

        public Int32 starttime = 0;	 /* test start time */
        public Int32 finishtime = 0; /* test end time */

        [SerializeLength(6)]
        public string mode = "";	/* test operating mode (enr., prod., etc.) */

        [SerializeLength(6)]
        public string tstrtyp = "";	/* production tester type */

        [SerializeLength(6)]
        public string tstrnum = "";	/* production tester number */

        [SerializeLength(6)]
        public string statnum = "";	/* tester station number */

        [SerializeLength(4)]
        public string flat = "";	/* wafer flat position (N, S, E, or W) */

        public Int16 xcen = 0;		/* x coordinate of wafer center */
        public Int16 ycen = 0;		/* y coordinate of wafer center */

        [SerializeLength(14)]
        public string swtyp = "";	/* type of test software */

        [SerializeLength(14)]
        public string swname = "";	/* name of test program */

        [SerializeLength(6)]
        public string swver = "";	/* revision number for test program */

        public Int32 swdate = 0;		/* date version was compiled */

        [SerializeLength(12)]
        public string parttyp = "";	/* plant code for location of testing */

        [SerializeLength(12)]
        public string partfmly = "";	/* family part number (for data storage) */

        [SerializeLength(12)]
        public string partdesig = "";	/* part designation */

        public Int16 xdim = 0;		/* x-die size in mils */
        public Int16 ydim = 0;		/* y-die size in mils */
        public Int16 bths = 0;		/* number of Batch records in file */
        public Int16 crrs = 0;		/* number of Correlation Results (crr) records in file */
        public Int16 bdcs = 0;		/* number of Bin Code Description (bdc) records in file */
        public Int16 tdcs = 0;      /* number of Test Description (tdc) records in file,Win 32 ushort*/
        public UInt16 ddcs = 0;		/* number of Device Description records (ddc) in file */
        public Int16 trcs = 0;		/* number of Test Results records (trc) per Test Description in file */
        public Int16 xlocmin = 0;	/* minimum xloc position from stepper */
        public Int16 xlocmax = 0;	/* maximum xloc position from stepper */
        public Int16 ylocmin = 0;	/* minimum yloc position from stepper */
        public Int16 ylocmax = 0;	/* maximum yloc position from stepper */

        [SerializeLength(6)]
        public string usvn = "";	/* UDF creation Software Version Number */

        [SerializeLength(6)]
        public string csvn = "";	/* CDF creation Software Version Number */

        public Int16 hdcs;          // Number of appended CDF files

        [SerializeLength(18)]
        public string reserved = ""; /* future (set to null if not used) */

        [SerializeLength(40)]
        public string catlist = "";	/* catalog part number */ 

        [SerializeLength(40)]
        public string customer = "";	/* customer number */

        [SerializeLength(40)]
        public string workorder = "";	/* work order number */

        [SerializeLength(6)]
        public string datecode = "";	/* date code as "yymmdd" */

        [SerializeLength(16)]
        public string wlot = "";	/* future (set to null if not used) */

        [SerializeLength(12)]
        public string wnum = "";	/* future (set to null if not used) */

        [SerializeLength(100)]
        public string future = "";	/* future (set to null if not used) */
    }
}
