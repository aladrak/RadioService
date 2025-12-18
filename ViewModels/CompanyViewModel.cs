using System.Collections.ObjectModel;
using Radiotech.Data;

namespace Radiotech.ViewModels;

public class CompanyViewModel
{
    public int FreeId => Companies.Count() != 0 ? Companies.Max(p => p.CompanyID) + 1 : 1;
    public ObservableCollection<TableData.Company> Companies
    {
        get => _companies; 
        private set => _companies = value; 
    }
    private ObservableCollection<TableData.Company> _companies = [];
    private readonly ITableRepository<TableData.Company> _repository;

    public CompanyViewModel()
    {
        _repository = new TableRepository<TableData.Company>("company.json");
        Companies = new ObservableCollection<TableData.Company>(_repository.GetAll());
    }
    public void Add(TableData.Company e)
    {
        Companies.Add(e);
        _repository.InsertOrUpdate(Companies.ToList());
    }
    public void Delete(TableData.Company e) // TODO: Checking dependencies
    {
        Companies.Remove(e);
        _repository.InsertOrUpdate(Companies.ToList());
    }
    public void Update(TableData.Company old, TableData.Company cur)
    {
        Companies[Companies.IndexOf(old)] = cur;
        _repository.InsertOrUpdate(Companies.ToList());
    }
}