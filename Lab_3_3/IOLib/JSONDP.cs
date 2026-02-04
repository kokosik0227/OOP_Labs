using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace IOLib
{
public class JsonProvider : DataProvider
{
public override void Save<T>(T data, string filePath)
{
    var directory = Path.GetDirectoryName(filePath);
    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        Directory.CreateDirectory(directory);

    File.WriteAllText(filePath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
}


public override T Load<T>(string filePath)
{
    if (!File.Exists(filePath))
        return Activator.CreateInstance<T>();

    return JsonSerializer.Deserialize<T>(File.ReadAllText(filePath))!;
}
}
}