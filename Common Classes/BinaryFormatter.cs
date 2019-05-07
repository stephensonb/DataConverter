using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DataConverter
{

    /// <summary>
    /// Default implementation of the IBinaryFormatter interface.
    /// <para>Subclass this class to get the default Serialization/Deserialization behavior.  You may override the serialization code for the Serialize, Deserialize methods.</para>
    /// <para>You can also override the Before/After methods.  The default implementations don't do anything.</para>   
    /// </summary>
    public class BinaryFormatter : IBinaryFormatter
    {
        // Serializes the class to the passed i/o stream.   The stream can be any valid stream type (File, Memory).
        /// <summary>
        /// Serializes the contents of the class instance to the passed i/o stream.  Only public fields are initialized.
        /// </summary>
        /// <param name="s">The stream to serialize the object instance to.</param>
        public virtual void Serialize(Stream s)
        {
            BinaryWriter bw = new BinaryWriter(s);
            
            // Call the before serialization method
            BeforeSerialize(s);
            
            bw.Write(GetBytes());

            // Call the aftger serialization method
            AfterSerialize(s);
        }

        /// <summary>
        /// Deserializes data from the given stream into the instance of the calling class.
        /// </summary>
        /// <param name="s">The stream to deserialize from.</param>
        /// <remarks>
        /// To deserialize data from a file "filetoread.bin" into a class MyClass that implements BinaryFormatter:
        ///
        /// MyClass data = new MyClass();
        /// StreamReader fr = new StreamReader("filetoread.bin");
        /// data.Deserialize(fr);  Will initialize the instance referenced by "data" with the data from file.
        /// </remarks>
        public void Deserialize(Stream s)
        {
            // Call the before deserialization method
            BeforeDeserialize(s); 

            // Using reflection, walk through the object and deserialize all public fields.
            foreach (FieldInfo f in this.GetType().GetFields())
            {
                DeserializeField(s, f.Name);
            }

            // Call the after serialization method
            AfterDeserialize(s);
        }

        /// <summary>
        /// Deserializes a field from an open stream at the current position in the stream.  If the stream data
        /// is not valid for the defined field, an exception may be thrown if the deserializer is not able to 
        /// coerce the data into the type represented by the field being deserialized.
        /// </summary>
        /// <param name="s">The stream to deserialize from.</param>
        /// <param name="fieldName">The name of the public field in the object to deserialize from the stream.</param>
        public void DeserializeField(Stream s, string fieldName)
        {
            BinaryReader bw = new BinaryReader(s, Encoding.ASCII);

            int attrsize = 0;
            object field;
            object[] attrs;
            SerializeLengthAttribute slen = null;

            // Search for the field in this object matching "fieldName"
            foreach (FieldInfo f in this.GetType().GetFields())
            {
                object fval = f.GetValue(this);
                Type ftype = f.FieldType;
                slen = null;

                attrs = f.GetCustomAttributes(typeof(SerializeLengthAttribute),false);
                
                if (attrs.Length > 0)
                {
                    // If the field has the SerializeLengthAttribute then get it so we know how many bytes to read.
                    slen = (SerializeLengthAttribute)attrs[0];
                }

                if (f.Name == fieldName)
                {
                    // If the number of bytes to read from the stream is defined, then set the size of the field.
                    if (slen != null)
                    {
                        attrsize = slen.Length;
                    }

                    if (f.FieldType.Name == "String")
                    {
                        // Read a string from the stream, stripping any null characters
                        string s1 = new String(bw.ReadChars(attrsize));
                        if (!s1.Contains("\0"))
                        {
                            field = s1;
                        }
                        else
                        {
                            field = s1.Substring(0 , s1.IndexOf("\0"));
                        }
                        f.SetValue(this, field);
                        break;
                    }

                    // If the field is an array then read in the number of elements
                    // into the appropriate array type.
                    if (f.FieldType.IsArray)
                    {
                        string em = f.FieldType.GetElementType().Name;
                        switch (em)
                        {
                            case "Byte":
                                field = bw.ReadBytes(attrsize);
                                f.SetValue(this, field);
                                break;
                            case "Char":
                                field = bw.ReadChars(attrsize);
                                f.SetValue(this, field);
                                break;
                        }
                        break;
                    }
                    else
                    {
                        // Try to read in and cast the data based on the field type.
                        // Note, if the data cannot be coerced into the correct data type
                        // an exception will be thrown on the ReadXXXX attempt.
                        string em = f.FieldType.Name;
                        switch (em)
                        {
                            case "Byte":
                                field = bw.ReadByte();
                                f.SetValue(this, field);
                                break;
                            case "SByte":
                                field = bw.ReadSByte();
                                f.SetValue(this, field);
                                break;
                            case "Boolean":
                                field = bw.ReadBoolean();
                                f.SetValue(this, field);
                                break;
                            case "Int16":
                                field = bw.ReadInt16();
                                f.SetValue(this, field);
                                break;
                            case "UInt16":
                                field = bw.ReadUInt16();
                                f.SetValue(this, field);
                                break;
                            case "Char":
                                field = bw.ReadChar();
                                f.SetValue(this, field);
                                break;
                            case "Int32":
                                field = bw.ReadInt32();
                                f.SetValue(this, field);
                                break;
                            case "UInt32":
                                field = bw.ReadUInt32();
                                f.SetValue(this, field);
                                break;
                            case "Single":
                                field = bw.ReadSingle();
                                f.SetValue(this, field);
                                break;
                            case "Int64":
                                field = bw.ReadInt64();
                                f.SetValue(this, field);
                                break;
                            case "UInt64":
                                field = bw.ReadUInt64();
                                f.SetValue(this, field);
                                break;
                            case "Double":
                                field = bw.ReadDouble();
                                f.SetValue(this, field);
                                break;
                            case "Decimal":
                                field = bw.ReadDecimal();
                                f.SetValue(this, field);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates and returns the total size of the serialized object in bytes.  
        /// </summary>
        /// <returns>The size, in bytes, of what the serialized data in the current object would be in the object's current state.</returns>
        public int GetSize()
        {
            int totalSize = 0;

            // Cycle through all of the public fields and calculate their sizes, summing them as we go.
            foreach (FieldInfo f in this.GetType().GetFields())
            {
                int size = 0;
                int attrsize = 0;
                object[] attrs;
                object fval = f.GetValue(this);
                Type ftype = f.FieldType;
                SerializeLengthAttribute slen = null;

                attrs = f.GetCustomAttributes(typeof(SerializeLengthAttribute),false);
                
                if (attrs.Length > 0)
                {
                    // If the field has the SerializeLengthAttribute then get it so we know how many bytes to read.
                    slen = (SerializeLengthAttribute)attrs[0];
                }
                
                if (slen != null)
                {
                    attrsize = slen.Length;
                }

                // If the field is an array and it is defined (not null) then get the size
                if (f.FieldType.IsArray && fval != null)
                {
                    string em = f.FieldType.GetElementType().Name;
                    switch (em)
                    {
                        case "Byte":
                            size = ((byte[])fval).Length;
                            break;
                        case "Char":
                            size = ((char[])fval).Length;
                            break;
                    }
                }
                // If the field is a List collection and the collection contains items
                // that are subclasses of BinaryStructure, the call the GetSize function on each
                // of the list elements to sum up the total size of the data in the list.
                else if (f.FieldType.Name.StartsWith("List"))
                {
                    if (f.FieldType.GetGenericArguments()[0].BaseType.Name == "BinaryFormatter")
                    {
                        foreach (BinaryFormatter bs in (IEnumerable<IBinaryFormatter>)fval)
                        {
                            size = size + bs.GetSize();
                        }
                    }
                }
                else
                {
                    // The field is a base type, so get its size based on the type definitions
                    string em = f.FieldType.Name;
                    switch (em)
                    {
                        case "String":
                            size = attrsize;
                            break;
                        case "Byte":
                            size = sizeof(Byte);
                            break;
                        case "SByte":
                            size = sizeof(SByte);
                            break;
                        case "Boolean":
                            size = sizeof(Boolean);
                            break;
                        case "Int16":
                            size = sizeof(Int16);
                            break;
                        case "UInt16":
                            size = sizeof(UInt16);
                            break;
                        case "Char":
                            size = sizeof(Char);
                            break;
                        case "Int32":
                            size = sizeof(Int32);
                            break;
                        case "UInt32":
                            size = sizeof(UInt32);
                            break;
                        case "Single":
                            size = sizeof(Single);
                            break;
                        case "Int64":
                            size = sizeof(Int64);
                            break;
                        case "UInt64":
                            size = sizeof(UInt64);
                            break;
                        case "Double":
                            size = sizeof(Double);
                            break;
                        case "Decimal":
                            size = sizeof(Decimal);
                            break;
                        default:
                            // If the type is not decipherable, then don't write it (size = 0);
                            size = 0;
                            break;
                    }
                }
                totalSize += size;
            }
            return totalSize;
        }


        /// <summary>
        /// Serializes the public fields of the object into an array of bytes.
        /// </summary>
        /// <returns>A byte array containing the serialized state of all of the public fields within the object.</returns>
        public byte[] GetBytes()
        {
            // Initialize the buffer with the size of the serialized stream
            byte[] bytes = new byte[GetSize()];
            
            // create a memory stream to buffer the serialization
            using(MemoryStream s = new MemoryStream(bytes.Length))
            {

                // Create a binary writer to write the serialized data to the underlying memory stream.
                using(BinaryWriter bw = new BinaryWriter(s, Encoding.ASCII))
                {

                    // Cycle through all of the public fields and serialize them to the memory stream
                    foreach (FieldInfo f in this.GetType().GetFields())
                    {
                        int attrsize = 0;
                        object fval = f.GetValue(this);
                        object[] attrs;
                        SerializeLengthAttribute slen = null;

                        if (fval == null)
                        {
                            continue;
                        }

                        Type ftype = f.FieldType;

                        attrs = f.GetCustomAttributes(typeof(SerializeLengthAttribute), false);
                        
                        if (attrs.Length>0)
                        {
                            slen = (SerializeLengthAttribute)attrs[0];
                        }

                        if (slen != null)
                        {
                            attrsize = slen.Length;
                        }

                        // Handle strings - If the length of the contents of the string is less than the
                        // length specified by the SerializeLength attribute, then pad the string with null (0x00).
                        if (f.FieldType.Name == "String")
                        {
                            string sval = (String)fval;
                            if (sval.Length < attrsize)
                            {
                                sval = sval.PadRight(attrsize, (Char)0x00);
                            }
                            bw.Write(sval.ToCharArray(0, attrsize));
                        }

                        if (f.FieldType.IsArray && fval != null)
                        {
                            string em = f.FieldType.GetElementType().Name;
                            switch (em)
                            {
                                case "Byte":
                                    bw.Write((Byte[])fval);
                                    break;
                                case "Char":
                                    bw.Write((Char[])fval);
                                    break;
                            }
                        }
                        // Serialize lists that hold elements subclassed from BinaryFormatter
                        else if (f.FieldType.Name.StartsWith("List"))
                        {
                            if (f.FieldType.GetGenericArguments()[0].BaseType.Name == "BinaryFormatter")
                            {
                                foreach (BinaryFormatter bs in (IEnumerable<IBinaryFormatter>)fval)
                                {
                                    bw.Write(bs.GetBytes());
                                }
                            }
                        }
                        else
                        {
                            // Serialize a field that is a base type
                            string em = f.FieldType.Name;
                            switch (em)
                            {
                                case "Byte":
                                    bw.Write((Byte)fval);
                                    break;
                                case "SByte":
                                    bw.Write((SByte)fval);
                                    break;
                                case "Boolean":
                                    bw.Write((Boolean)fval);
                                    break;
                                case "Int16":
                                    bw.Write((Int16)fval);
                                    break;
                                case "UInt16":
                                    bw.Write((UInt16)fval);
                                    break;
                                case "Char":
                                    bw.Write((Char)fval);
                                    break;
                                case "Int32":
                                    bw.Write((Int32)fval);
                                    break;
                                case "UInt32":
                                    bw.Write((UInt32)fval);
                                    break;
                                case "Single":
                                    bw.Write((Single)fval);
                                    break;
                                case "Int64":
                                    bw.Write((Int64)fval);
                                    break;
                                case "UInt64":
                                    bw.Write((UInt64)fval);
                                    break;
                                case "Double":
                                    bw.Write((Double)fval);
                                    break;
                                case "Decimal":
                                    bw.Write((Decimal)fval);
                                    break;
                            }
                        }
                    }

                    // Make sure all data written to the underlying memory stream
                    bw.Flush();

                    // Get the bytes from the memory stream
                    bytes = s.ToArray();

                } // Close the BinaryWriter using and release all of its resources

            } // Close the MemoryStream using and release all of its resources
            
            // Return the serialized data as a byte array
            return bytes;
        }

        /// <summary>
        /// Called before serialization begins to allow a subclass to do special processing if needed
        /// </summary>
        /// <param name="s"></param>
        public virtual void BeforeSerialize(Stream s)
        {
            // Default implementation does nothing
        }

        /// <summary>
        /// Called after serialization to allow a subclass to do special processing if needed
        /// </summary>
        /// <param name="s"></param>
        public virtual void AfterSerialize(Stream s)
        {
            // Default implementation does nothing
        }

        /// <summary>
        /// Called before deserialization begins to allow a subclass to do special processing if needed
        /// </summary>
        /// <param name="s"></param>
        public virtual void BeforeDeserialize(Stream s)
        {
            // Default implementation does nothing
        }

        /// <summary>
        /// Called after deserialization to allow a subclass to do special processing if needed
        /// </summary>
        /// <param name="s"></param>
        public virtual void AfterDeserialize(Stream s)
        {
            // Default implementation does nothing
        }
    }
}
