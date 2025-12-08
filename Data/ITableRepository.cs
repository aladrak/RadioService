using System.Collections.ObjectModel;

namespace Radiotech.Data;

public interface ITableRepository
{
    public interface ITableRepository<T>
    {
        // Person FindById (string id);
        //
        // bool IsFavorite (Person person);
        //
        void InsertOrUpdate(ObservableCollection<T> e);
        //
        // void Delete (Person person);

        event EventHandler Changed;
    }
}