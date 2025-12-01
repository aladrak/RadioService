using System.Text.Json;

namespace Radiotech.Data;

public class JsonDataStore<T> : IDataStore<T>
{
    private readonly string _dataPath;

    public JsonDataStore(string dataPath)
    {
        _dataPath = dataPath;
    }

    public T Load()
    {
        if (!File.Exists(_dataPath))
            return Activator.CreateInstance<T>();
        using Stream stream = File.Open(_dataPath, FileMode.OpenOrCreate);
        return JsonSerializer.Deserialize<T>(stream) 
               ?? Activator.CreateInstance<T>();
    }

    public void Save(T data)
    {
        try
        {
            if (!File.Exists(_dataPath)) throw new Exception("The file does not exist");
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dataPath, json);
        }
        catch (Exception ex) { /* Nothing here */ }
    }
}