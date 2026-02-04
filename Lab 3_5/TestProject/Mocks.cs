using System;
using IOLib;

namespace Tests.Mocks
{   public class MockProvider : DataProvider
    {
        public object StoredData { get; set; }

        public override void Save<T>(T data, string filePath)
        {
            StoredData = data;
        }

        public override T Load<T>(string filePath)
        {
            if (StoredData is T t)
                return t;

            return Activator.CreateInstance<T>();
        }
    }
}