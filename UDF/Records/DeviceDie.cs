using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tronics.DataConverter.UDF
{
    public class DeviceDie : BinaryFormatter
    {
        public Devdr DieDescription;
        public List<Tmeas> TestMeasurements;
    }
}
