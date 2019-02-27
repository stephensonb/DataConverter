﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tronics.DataConverter.CDF
{
    /// <summary>
    /// Bdc Record Definition
    /// </summary>
    /// <remarks>
    /// Bin Description Record format per CDF specification.
    /// </remarks>

    public class Bdc : BinaryFormatter
    {
        [SerializeLength(24)]
        public string binname = "";	/* bin name */
        public Int16 binnum=0;		/* bin number */
        public Int32 bincnt=0;		/* number of parts in bin */
        [SerializeLength(8)]
        public string future = "";	/* extra space for future expansion */
    }
}