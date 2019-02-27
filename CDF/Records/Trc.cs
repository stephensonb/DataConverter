using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tronics.DataConverter.CDF
{
    /// <summary>
    /// Trc Record Definition
    /// </summary>
    /// <remarks>
    /// Test Results record definition per CDF specification
    /// </remarks>
    public class Trc : BinaryFormatter
    {
        public object result = (ushort)0xFFFF;

        /// <summary>
        /// GetResult - Gets the result value for a test, cast into a CDF data type.
        /// </summary>
        /// <param name="datatype">Specifies the CDF defined two character datatype specifier to return the result value cast into the correct data type.</param>
        /// <returns>object - returns the result cast into the desired data type.  You must cast the return value from an object into the correct datatype or use untyped variable to store the return.</returns>
        public object GetResult(String datatype)
        {
            switch (datatype)
            {
                case "CF":
                case "I%":
                    return (Int16)result;
                case "I&":
                    return (Int32)result;
                case "F!":
                    return (Single)result;
                case "F#":
                    return (Double)result;
                case "S$":
                    return (String)result;
                default:
                    return (Int16)result;
            }
        }

        /// <summary>
        /// SetResult - Sets the result value for a test.
        /// </summary>
        /// <param name="datatype">Specifies the CDF defined datatype specifier and casts the passed result into the correct dataype for storage.</param>
        /// <param name="res">object containing the test result value to be stored.</param>
        public void SetResult(String datatype, object res)
        {
            switch (datatype)
            {
                case "CF":
                case "I%":
                    result = (Int16)res;
                    break;
                case "I&":
                    result = (Int32)res;
                    break;
                case "F!":
                    result = (Single)res;
                    break;
                case "F#":
                    result = (Double)res;
                    break;
                case "S$":
                    result = (String)res;
                    break;
                default:
                    result = (Int16)res;
                    break;
            }
        }

        /// <summary>
        /// Serialize - Prepares the record for serialization.  The format of the serialized data depends on the CDF format specified.
        /// </summary>
        /// <param name="s">The <i>stream</i> that the data is to be serialized to.</param>
        /// <param name="td">A Test Descriptor record that contains the data to be serialized.</param>
        public void Serialize(Stream s, Tdc td)
        {
            BinaryWriter bw = new BinaryWriter(s);

            switch (td.datatype)
            {
                case "CF":
                case "I%":
                    bw.Write((Int16)result);
                    break;
                case "I&":
                    bw.Write((Int32)result);
                    break;
                case "F!":
                    bw.Write((Single)result);
                    break;
                case "F#":
                    bw.Write((Double)result);
                    break;
                case "S$":
                    bw.Write(((String)result).ToCharArray(), 0, td.datasizeb);
                    break;
            }
        }

        /// <summary>
        /// Deserialize - Deserializes data from the stream to the passed Test Descriptor object.  The format and length of the data to be read from the stream is specified by the CDF type specifier in the Test Descriptor record.
        /// </summary>
        /// <param name="s">The <i>stream</i> object that the data is to be read from.</param>
        /// <param name="td">A Test Descriptor record where the data is to be stored.</param>
        public void Deserialize(Stream s, Tdc td)
        {
            BinaryReader br = new BinaryReader(s);
            switch (td.datatype)
            {
                case "CF":
                case "I%":
                    result = br.ReadInt16();
                    break;
                case "I&":
                    result = br.ReadInt32();
                    break;
                case "F!":
                    result = br.ReadSingle();
                    break;
                case "F#":
                    result = br.ReadDouble();
                    break;
                case "S$":
                    result = new String(br.ReadChars(td.datasizeb));
                    break;
            }
        }
    }
}