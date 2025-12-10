using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Radiotech.Data;

namespace Radiotech.ViewModels;

public class PersonViewModel
{
    public ObservableCollection<TableData.Person> Persons 
    {
        get => _persons; 
        private set => SetProperty(ref _people, value); 
    }
    private ObservableCollection<TableData.Person> _persons = [];
    private readonly ITableRepository<TableData.Person> _repository;

    public PersonViewModel()
    {
        _repository = new TableRepository<TableData.Person>("person.json");
        Persons = new ObservableCollection<TableData.Person>(_repository.GetAll());
    }
    public void SavePersons()
    {
        _repository.InsertOrUpdate(Persons.ToList());
    }
}