using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tronics.DataConverter
{
    interface IKLARFWriter
    {
        bool WriteKLARF(string path);
    }
}
