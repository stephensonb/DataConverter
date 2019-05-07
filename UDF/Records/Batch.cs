namespace DataConverter.UDF
{
    public class Batch : BinaryFormatter
    {
        [SerializeLength(16)]
        public string lot = "";	/* lot designation */
        [SerializeLength(12)]
        public string sublot = "";	/* sublot designation */
        [SerializeLength(16)]
        public string future = "";	/* extra space for future expansion */
    }
}
