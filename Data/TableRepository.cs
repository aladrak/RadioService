using System.Text.Json;

namespace Radiotech.Data
{
	public class TableRepository<T> : ITableRepository<T>
	{
		private const string appName = "RadioService";
		private static string _appFolder;
		private readonly string _filePath;
		// private const string PERSON_PATH = "person.json";
		//private const string COMPANY_PATH = "company.json";
		//private const string PRODUCT_PATH = "product.json";
		//private const string SPECIALTY_PATH = "specialty.json";
		//private const string EMPLOYEE_PATH = "employee.json";
		//private const string ORDER_PATH = "order.json";
		static TableRepository() 
		{
			_appFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), appName);
			Directory.CreateDirectory(_appFolder);
		}

		public TableRepository(string filePath)
		{
			_filePath = Path.Combine(_appFolder, filePath);
		}
		// private void DataCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		// {
		// 	switch (e.Action)
		// 	{
		// 		case NotifyCollectionChangedAction.Add:
		// 			InsertOrUpdate(Data);
		// 			break;
		// 		case NotifyCollectionChangedAction.Remove:
		// 			InsertOrUpdate(Data);
		// 			break;
		// 		case NotifyCollectionChangedAction.Replace:
                
		// 			break;
		// 	}
		// }
		public List<T> GetAll()
		{
			if (!File.Exists(_filePath))
				return Activator.CreateInstance<List<T>>();
			using Stream stream = File.Open(_filePath, FileMode.OpenOrCreate);
			// List = JsonSerializer.Deserialize<ObservableCollection<T>>(stream) 
			//        ?? Activator.CreateInstance<ObservableCollection<T>>();
			return JsonSerializer.Deserialize<List<T>>(stream) 
			       ?? Activator.CreateInstance<List<T>>();
		}
		
		public void InsertOrUpdate(List<T> data)
		{
			try
			{
				if (!File.Exists(_filePath)) throw new Exception("The file does not exist");
				string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(_filePath, json);
			}
			catch (Exception ex) { /* Nothing here */ }
		}
		
		// public void Delete(T data)
		// {
		// 	var newPeopleQ = from p in People
		// 		where p.Id != person.Id
		// 		select p;
		// 	var newPeople = newPeopleQ.ToList();
		// 	var n = People.Count - newPeople.Count;
		// 	People = newPeople;
		// 	if (n != 0)
		// 	{
		// 		var task = Task.Run(async () =>
		// 		{
		// 			await Commit();
		// 		});
		// 		task.Wait();
		// 	}
		// }
		
		// private async Task Commit()
		// {
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
	}
}
