using System;

namespace DataConverter.KLARF
{
    public class DefectClass
    {
        public Int32 ClassNumber;
        public string ClassName;

        public override string ToString()
        {
            return string.Format("{0} {1}\n\r", ClassNumber, ClassName);
        }
    }

}
