namespace DataConverter.KLARF
{
    public class DiePitch
    {
        public double X;
        public double Y;

        public override string ToString()
        {
            return string.Format("{0:f3} {1:f3}\n\r", X, Y);
        }
    }

}
