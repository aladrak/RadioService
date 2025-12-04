using System.Collections.ObjectModel;

namespace Radiotech.Data;

public interface IDataStore<T>
{
    ObservableCollection<T> Data { get; set; }
    ObservableCollection<T> Load();
    void Save(ObservableCollection<T> data);
}