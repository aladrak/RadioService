namespace Radiotech.Data;

public interface IDataStore<T>
{
    T Load();
    void Save(T data);
}