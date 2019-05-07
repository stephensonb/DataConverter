using System.Collections.Generic;

namespace DataConverter.KLARF
{
    public class KLARFile
    {
        public FileVersionRecord FileVersion;
        public FileTimestampRecord FileTimestamp;
        public TiffSpecRecord TiffSpec;
        public InspectionStationRecord InspectionStationID;
        public string SampleType;
        public ResultTimestampRecord ResultTimeStamp;
        public string ResultsID;
        public List<LotRecord> Lots;
    }
}
