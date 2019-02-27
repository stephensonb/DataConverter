using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Tronics.DataConverter
{
    /// <summary>
    /// The IBinaryFormatter interface defines the methods implemented for serializing and deserializing public fields in an object.
    /// </summary>
    public interface IBinaryFormatter
    {
        void Serialize(Stream s);
        void Deserialize(Stream s);
        void DeserializeField(Stream s, string fieldName);
        void BeforeSerialize(Stream s);
        void AfterSerialize(Stream s);
        void BeforeDeserialize(Stream s);
        void AfterDeserialize(Stream s);
        int GetSize();
        byte[] GetBytes();
    }
}
