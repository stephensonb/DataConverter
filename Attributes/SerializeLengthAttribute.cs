using System;

namespace DataConverter
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
