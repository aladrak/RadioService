using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.Json;

namespace Radiotech.Data;

public class JsonDataStore<T>
{
    public ObservableCollection<T> Data { get; set; }
    private readonly string _dataPath;
    public JsonDataStore(string dataPath)
    {
        _dataPath = dataPath;
        Data = Load();
        Data.CollectionChanged += DataCollectionChanged;
    }

    public ObservableCollection<T> Load()
    {
        if (!File.Exists(_dataPath))
            return Activator.CreateInstance<ObservableCollection<T>>();
        using Stream stream = File.Open(_dataPath, FileMode.OpenOrCreate);
        return JsonSerializer.Deserialize<ObservableCollection<T>>(stream) 
               ?? Activator.CreateInstance<ObservableCollection<T>>();
    }

    public void Save(ObservableCollection<T> data)
    {
        try
        {
            if (!File.Exists(_dataPath)) throw new Exception("The file does not exist");
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dataPath, json);
        }
        catch (Exception ex) { /* Nothing here */ }
    }
    private void DataCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                Save(Data);
                break;
            case NotifyCollectionChangedAction.Remove:
                Save(Data);
                break;
            case NotifyCollectionChangedAction.Replace:
                
                break;
        }
    }
}