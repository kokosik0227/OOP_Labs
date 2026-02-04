using System.Collections.Generic;


namespace IOLib
{
public abstract class DataProvider
{
    public abstract void Save<T>(T data, string filePath);
    public abstract T Load<T>(string filePath);
}
}