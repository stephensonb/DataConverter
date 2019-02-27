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
    /// Bth Record Definition
    /// </summary>
    /// <remarks>
    /// Batch Header record definition per CDF specification
    /// </remarks>
    public class Bth : BinaryFormatter
    {
        [SerializeLength(16)]
        public string lot = "";	/* lot designation */
        [SerializeLength(12)]
        public string sublot = "";	/* sublot designation */
        [SerializeLength(16)]
        public string future = "";	/* extra space for future expansion */
    }
}
