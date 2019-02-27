using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tronics.DataConverter.UDF
{
    public class Tmeas : BinaryFormatter
    {
        public float Value = 0;
        public Int16 Flags = 0x00;
    }
}
