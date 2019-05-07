using System;

namespace DataConverter.KLARF
{
    public class InspectionStationRecord
    {
        public string ManufacturerID="";
        public string Model="";
        public string EquipmentID="";

        public override string ToString()
        {
            return String.Format("InspectionStationID \"{0}\" \"{1}\" \"{2}\";\n\r", ManufacturerID, Model, EquipmentID);
        }

    }

}
