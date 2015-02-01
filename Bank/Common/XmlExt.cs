using System.IO;
using System.Xml.Serialization;

namespace Common
{
    static public class XmlExt
    {
        static public string ToXml<T>(this T e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringWriter textWriter = new StringWriter();
            serializer.Serialize(textWriter, e);
            return textWriter.ToString();
        }

        static public void FromXml<T>(this T e, string str)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader textReader = new StringReader(str);
            e = (T) deserializer.Deserialize(textReader);
        }
    }
}
