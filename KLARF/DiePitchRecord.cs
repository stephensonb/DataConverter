namespace DataConverter.KLARF
{
    public class DiePitchRecord
    {
        public float X;
        public float Y;

        public override string ToString()
        {
            return string.Format("DiePitch {0} {1};\n\r", X, Y);
        }
    }

}
