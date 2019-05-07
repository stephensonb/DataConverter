using System;

namespace DataConverter.KLARF
{
    public class InspectedAreaRecord
    {
        public Int32 Count;
        public InspectedArea[] InspectedAreas;

        public InspectedAreaRecord(string count)
        {

            Count = Int32.Parse(count);
            InspectedAreas = new InspectedArea[Count];
        }
    }

}
