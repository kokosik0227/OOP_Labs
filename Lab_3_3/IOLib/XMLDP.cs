using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace IOLib
{
public class XmlProvider : DataProvider
{
public override void Save<T>(T data, string filePath)
        {
            var xs = new XmlSerializer(typeof(T));
            using var fs = new FileStream(filePath, FileMode.Create);
            xs.Serialize(fs, data);
        }

public override T Load<T>(string filePath)
{
    if (!File.Exists(filePath))
        return Activator.CreateInstance<T>();

    var xs = new XmlSerializer(typeof(T));
    using var fs = new FileStream(filePath, FileMode.Open);
    return (T)xs.Deserialize(fs)!;
}
}
}