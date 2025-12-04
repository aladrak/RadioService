
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Radiotech.Data
{
	public class TableRepository : ITableRepository
	{
		private const string appName = "RadioService";
		private static string _folder;
		private readonly IDataStore<TableData.Person> _personData;
		public ObservableCollection<TableData.Person> PersonData { get => _personData.Data; set => _personData.Data = value; }
		// private const string PERSON_PATH = "person.json";
		//private const string COMPANY_PATH = "Company.json";
		//private const string PRODUCT_PATH = "Product.json";
		//private const string SPECIALTY_PATH = "Specialty.json";
		//private const string EMPLOYEE_PATH = "Employee.json";
		//private const string ORDER_PATH = "Order.json";
		public TableRepository()
		{
			_folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), appName);
			Directory.CreateDirectory(_folder);
			
			_personData = new JsonDataStore<TableData.Person>(Path.Combine(_folder, "person.json"));
		}
		
		// private async Task Commit()
		// {
		// 	
		//
		// 	try
		// 	{
		// 		var filePath = Path.Combine(FileSystem.AppDataDirectory, IsolatedStorageName);
		// 		Directory.CreateDirectory(Path.GetDirectoryName(filePath));
  //               
		// 		using var f = new FileStream(filePath, FileMode.Create, FileAccess.Write);
		// 		serializer.Serialize(f, this);
		// 	}
		// 	catch (Exception)
		// 	{
		// 		// Ignore serialization errors
		// 	}
		//
		// 	// var ev = Changed;
		// 	// if (ev != null)
		// 	// {
		// 	// 	ev(this, EventArgs.Empty);
		// 	// }
		// }

		// public void InsertOrUpdate( e)
		// {
		// 	var path = Path.Combine(_folder, e.Path);
		// 	try
		// 	{
		// 		if (!File.Exists(path)) throw new Exception("The file does not exist");
		// 		string json = JsonSerializer.Serialize(e, options);
		// 		File.WriteAllText(path, json);
		// 	}
		// 	catch (Exception ex) { Console.WriteLine(ex); }
		// }
	}
}
