using System.Collections.ObjectModel;
using Radiotech.Data;

namespace Radiotech.ViewModels;

public class MainViewModel
{
    public ObservableCollection<TableData.Person> Persons { get; set; }
    public TableRepository TableRepository { get; private set; }

    public MainViewModel()
    {
        TableRepository = new TableRepository();
        Persons = TableRepository.PersonData;
    }
}