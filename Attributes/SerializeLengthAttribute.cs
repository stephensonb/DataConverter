using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tronics.DataConverter
{
    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SerializeLengthAttribute : System.Attribute
    {
        public int Length;

        public SerializeLengthAttribute(int length)
        {
            Length = length;
        }
    }
}
