using System.Collections.ObjectModel;
using Radiotech.Data;

namespace Radiotech.ViewModels;

public class PersonViewModel
{
    public int FreeId => Persons.Count() != 0 ? Persons.Max(p => p.PersonID) + 1 : 1;
    public ObservableCollection<TableData.Person> Persons 
    {
        get => _persons; 
        private set => _persons = value; 
    }
    private ObservableCollection<TableData.Person> _persons = [];
    private readonly ITableRepository<TableData.Person> _repository;

    public PersonViewModel()
    {
        _repository = new TableRepository<TableData.Person>("person.json");
        Persons = new ObservableCollection<TableData.Person>(_repository.GetAll());
    }
    public void Add(TableData.Person person)
    {
        Persons.Add(person);
        _repository.InsertOrUpdate(Persons.ToList());
    }
    public void Delete(TableData.Person person) // TODO: Checking dependencies
    {
        Persons.Remove(person);
        _repository.InsertOrUpdate(Persons.ToList());
    }
    public void Update(TableData.Person old, TableData.Person cur)
    {
        Persons[Persons.IndexOf(old)] = cur;
        _repository.InsertOrUpdate(Persons.ToList());
    }
}