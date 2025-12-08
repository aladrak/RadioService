using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.Json;

namespace Radiotech.Data
{
	public class TableRepository<T> : ITableRepository
	{
		public ObservableCollection<T> Data;
		private const string appName = "RadioService";
		private readonly string _folder;
		// private const string PERSON_PATH = "person.json";
		//private const string COMPANY_PATH = "company.json";
		//private const string PRODUCT_PATH = "product.json";
		//private const string SPECIALTY_PATH = "specialty.json";
		//private const string EMPLOYEE_PATH = "employee.json";
		//private const string ORDER_PATH = "order.json";
		public TableRepository(string filePath)
		{
			var appFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), appName);
			Directory.CreateDirectory(appFolder);
			_folder = Path.Combine(appFolder, filePath);
			Data = new ObservableCollection<T>();
		}
		private void DataCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					InsertOrUpdate(Data);
					break;
				case NotifyCollectionChangedAction.Remove:
					InsertOrUpdate(Data);
					break;
				case NotifyCollectionChangedAction.Replace:
                
					break;
			}
		}
		public ObservableCollection<T> Load()
		{
			if (!File.Exists(_folder))
				return Activator.CreateInstance<ObservableCollection<T>>();
			using Stream stream = File.Open(_folder, FileMode.OpenOrCreate);
			// List = JsonSerializer.Deserialize<ObservableCollection<T>>(stream) 
			//        ?? Activator.CreateInstance<ObservableCollection<T>>();
			return JsonSerializer.Deserialize<ObservableCollection<T>>(stream) 
			       ?? Activator.CreateInstance<ObservableCollection<T>>();
		}
		
		public void InsertOrUpdate(ObservableCollection<T> data)
		{
			try
			{
				if (!File.Exists(_folder)) throw new Exception("The file does not exist");
				string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(_folder, json);
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
