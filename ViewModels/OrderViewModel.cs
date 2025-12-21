using System.Collections.ObjectModel;
using Radiotech.Data;

namespace Radiotech.ViewModels;

public class OrderViewModel
{
    public int FreeId => Orders.Count() != 0 ? Orders.Max(p => p.OrderID) + 1 : 1;
    public ObservableCollection<TableData.Order> Orders
    {
        get => _orders; 
        private set => _orders = value; 
    }
    public List<TableData.Product> Products { get; private set; }
    public List<TableData.Employee> Employees { get; private set; }
    public List<TableData.Company> Companies { get; private set; }
    public List<TableData.Person> Persons { get; private set; }
    private ObservableCollection<TableData.Order> _orders = [];
    private readonly ITableRepository<TableData.Order> _repository;

    public OrderViewModel()
    {
        _repository = new TableRepository<TableData.Order>("order.json");
        Orders = new ObservableCollection<TableData.Order>(_repository.GetAll());
        
        Products = new TableRepository<TableData.Product>("products.json").GetAll();
        Employees = new TableRepository<TableData.Employee>("employee.json").GetAll();
        Companies = new TableRepository<TableData.Company>("company.json").GetAll();
        Persons = new TableRepository<TableData.Person>("person.json").GetAll();
    }
    public void Add(TableData.Order e)
    {
        Orders.Add(e);
        _repository.InsertOrUpdate(Orders.ToList());
    }
    public void Delete(TableData.Order e) // TODO: Checking dependencies
    {
        Orders.Remove(e);
        _repository.InsertOrUpdate(Orders.ToList());
    }
    public void Update(TableData.Order old, TableData.Order cur)
    {
        Orders[Orders.IndexOf(old)] = cur;
        _repository.InsertOrUpdate(Orders.ToList());
    }
}