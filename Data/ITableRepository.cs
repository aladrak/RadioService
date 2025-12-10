using System.Collections.ObjectModel;

namespace Radiotech.Data;

public interface ITableRepository<T>
{
    // Person FindById (string id);
    void InsertOrUpdate(List<T> e);
    List<T> GetAll();
    //
    // void Delete (Person person);

    // event EventHandler Changed;
}
