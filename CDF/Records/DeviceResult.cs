using System.Collections.Generic;

namespace DataConverter.CDF
{
    /// <summary>
    /// DeviceResult record definition
    /// </summary>
    /// <remarks>
    /// Record that incorporates a die descriptor and the die results
    /// </remarks>

    public class DeviceResult : BinaryFormatter
    {
        public Ddc DieDescription;
        public List<Trc> TestMeasurements;

        public DeviceResult()
        {
            DieDescription = new Ddc();
            TestMeasurements = new List<Trc>();
        }
    }
}
