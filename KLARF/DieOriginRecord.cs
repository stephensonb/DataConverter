namespace DataConverter.KLARF
{
    public class DieOriginRecord
    {
        public float X;
        public float Y;

        public override string ToString()
        {
            return string.Format("DieOrigin {0} {1};\n\r", X, Y);
        }
    }

}
