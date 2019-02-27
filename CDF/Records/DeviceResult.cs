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
    /// DeviceResult record definition
    /// </summary>
    /// <remarks>
    /// Record that incorporates a die descriptor and the die results
    /// </remarks>

    public class DeviceResult : BinaryFormatter
    {
        public Ddc DieDescription;
        public List<Trc> TestMeasurements;

        public DeviceResult()
        {
            DieDescription = new Ddc();
            TestMeasurements = new List<Trc>();
        }
    }
}
