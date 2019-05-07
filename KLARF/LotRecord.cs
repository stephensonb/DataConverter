using System;
using System.Collections.Generic;
using System.IO;

namespace DataConverter.KLARF
{
    public class LotRecord
    {
        public string LotID = "";
        public SampleSizeRecord SampleSize;
        public SetupIDRecord SetupID;
        public string DeviceID = "";
        public string StepID = "";
        public string SampleOrientationMarkType = "";
        public string OrientationMarkLocation = "";
        public List<WaferRecord> Wafers = new List<WaferRecord>();
        public DiePitchRecord DiePitch;
        public DieOriginRecord DieOrigin;
        public LotStatusRecord LotStatus;
        public string WaferStatus = "";

        public void Serialize(StreamWriter fs)
        {
            fs.Write(String.Format("LotID {0};\n\r", LotID));
            fs.Write(SampleSize.ToString());
            fs.Write(SetupID.ToString());
            fs.Write(String.Format("DeviceID {0};\n\r", DeviceID));
            fs.Write(String.Format("StepID {0};\n\r", StepID));
            fs.Write(String.Format("SampleOrientationMarkType {0};\n\r", SampleOrientationMarkType));
            fs.Write(String.Format("OrientationMarkLocation {0};\n\r", OrientationMarkLocation));
            fs.Write(DiePitch.ToString());
            fs.Write(DieOrigin.ToString());
            foreach (WaferRecord wr in Wafers)
            {
                wr.Serialize(fs);
            }
            fs.Write(LotStatus.ToString());
            fs.Write(String.Format("WaferStatus {0};\n\r", WaferStatus));
        }
    }

}
