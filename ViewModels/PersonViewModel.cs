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
    }
}