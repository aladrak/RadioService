using System.Collections.ObjectModel;
using Radiotech.Data;

namespace Radiotech.ViewModels;

public class EmployeeViewModel
{
    public int FreeId => Employees.Max(p => p.EmployeeID) + 1;
    public ObservableCollection<TableData.Employee> Employees
    {
        get => _employees;
        private set => _employees = value; 
    }
    private ObservableCollection<TableData.Employee> _employees = [];
    private readonly ITableRepository<TableData.Employee> _repository;

    public EmployeeViewModel()
    {
        _repository = new TableRepository<TableData.Employee>("employee.json");
        Employees = new ObservableCollection<TableData.Employee>(_repository.GetAll());
    }
    public void Add(TableData.Employee e)
    {
        Employees.Add(e);
        _repository.InsertOrUpdate(Employees.ToList());
    }
    public void Delete(TableData.Employee e) // TODO: Checking dependencies
    {
        Employees.Remove(e);
        _repository.InsertOrUpdate(Employees.ToList());
    }
    public void Update(TableData.Employee e)
    {
        Employees.Insert(Employees.IndexOf(e), e);
        _repository.InsertOrUpdate(Employees.ToList());
    }
}