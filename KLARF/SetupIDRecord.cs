using System;

namespace DataConverter.KLARF
{
    public class SetupIDRecord
    {
        public string Name="";
        public string Date="";
        public string Time="";

        public override string ToString()
        {
            return String.Format("SetupID \"{0}\" {1} {2};\n\r", Name, Date, Time);
        }
    
    }

}
