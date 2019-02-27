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
	Device description record (Devdr):
    One Devdr is required for each device tested.
    */

    public class Devdr : BinaryFormatter
    {
        public Int32 dienum=0;	/* die designation number */
        public Int16 xpos=0;		/* x-coord (or LSBs of device number */
        public Int16 ypos=0;		/* y-coord (or MSBs of device number */
        public Int16 bincode=0;	/* bin code of device (after test) */
    }
}
