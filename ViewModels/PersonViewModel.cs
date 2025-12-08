using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Radiotech.Data;

namespace Radiotech.ViewModels;

public class PersonViewModel
{
    public ObservableCollection<TableData.Person> Persons { get; set; }
    private readonly TableRepository<TableData.Person> _tableRepo;

    public PersonViewModel()
    {
        _tableRepo = new TableRepository<TableData.Person>("person.json");
        Persons = new ObservableCollection<TableData.Person>();
        Persons = _tableRepo.Load();
        Persons.CollectionChanged += DataCollectionChanged;
    }
    private void DataCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                _tableRepo.InsertOrUpdate(Persons);
                break;
            case NotifyCollectionChangedAction.Remove:
                _tableRepo.InsertOrUpdate(Persons);
                break;
            case NotifyCollectionChangedAction.Replace:
                
                break;
        }
    }
}