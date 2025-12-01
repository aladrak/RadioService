namespace Radiotech.Data;

public interface ITableRepository
{
    public interface IFavoritesRepository
    {
        // IEnumerable<Person> GetAll ();
        //
        // Person FindById (string id);
        //
        // bool IsFavorite (Person person);
        //
        // void InsertOrUpdate (IDataClass e);
        //
        // void Delete (Person person);

        event EventHandler Changed;
    }
}