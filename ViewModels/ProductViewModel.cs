using System.Collections.ObjectModel;
using Radiotech.Data;

namespace Radiotech.ViewModels;

public class ProductViewModel
{
    public int FreeId => Products.Count() != 0 ? Products.Max(p => p.ProductID) + 1 : 1;
    public ObservableCollection<TableData.Product> Products
    {
        get => _products; 
        private set => _products = value; 
    }
    private ObservableCollection<TableData.Product> _products = [];
    private readonly ITableRepository<TableData.Product> _repository;

    public ProductViewModel()
    {
        _repository = new TableRepository<TableData.Product>("product.json");
        Products = new ObservableCollection<TableData.Product>(_repository.GetAll());
    }
    public void Add(TableData.Product e)
    {
        Products.Add(e);
        _repository.InsertOrUpdate(Products.ToList());
    }
    public void Delete(TableData.Product e) // TODO: Checking dependencies
    {
        Products.Remove(e);
        _repository.InsertOrUpdate(Products.ToList());
    }
    public void Update(TableData.Product old, TableData.Product cur)
    {
        Products[Products.IndexOf(old)] = cur;
        _repository.InsertOrUpdate(Products.ToList());
    }
}