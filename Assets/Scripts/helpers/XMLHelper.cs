using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Helpers
{
public class XMLHelper
{
    public static void Serialize(object obj, string path) {
        // open the streams and configure serializaztion
        XmlSerializer serializer = new XmlSerializer(obj.GetType());   
        StreamWriter writer = new StreamWriter(path);

        // Debug.Log($"{Path.GetFullPath(path)}");

        // write to the file 
        serializer.Serialize(writer.BaseStream, obj);

        // close the stream
        writer.Close();
    }

    public static T Deserialize<T>(string file_contents) {
        // open the streams and configure the serialization for this type of object
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StringReader reader = new StringReader(file_contents);

        // deserialize and close 
        T deserialized = (T) serializer.Deserialize(reader);
        reader.Close();

        // return new object
        return deserialized;
    }
}

} // Helpers 