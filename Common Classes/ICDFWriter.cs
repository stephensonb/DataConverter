using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tronics.DataConverter
{
    interface ICDFWriter
    {
        bool WriteCDF(string path);
    }
}
