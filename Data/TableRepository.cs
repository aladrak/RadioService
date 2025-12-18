using Newtonsoft.Json;

namespace Radiotech.Data;

public class TableRepository<T> : ITableRepository<T>
{
	private const string appName = "RadioService";
	private static string _appFolder;
	private readonly string _filePath;
	static TableRepository() 
	{
		_appFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), appName);
		Directory.CreateDirectory(_appFolder);
	}

	public TableRepository(string filePath)
	{
		_filePath = Path.Combine(_appFolder, filePath);
	}
	public List<T> GetAll()
	{
		try 
		{
			using Stream stream = File.Open(_filePath, FileMode.OpenOrCreate);
			var streamRead = new StreamReader(stream).ReadToEnd();
			
			return JsonConvert.DeserializeObject<List<T>>(streamRead)
				?? Activator.CreateInstance<List<T>>();
		}
		catch (Exception ex)
		{
			return Activator.CreateInstance<List<T>>();
		}
	}
	
	public void InsertOrUpdate(List<T> data)
	{
		try
		{
			if (!File.Exists(_filePath)) throw new Exception("The file does not exist");
			string json = JsonConvert.SerializeObject(data, Formatting.Indented);
				// data, 
				// new JsonSerializerOptions
				// {
				// 	Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, 
				// 	WriteIndented = true,
				// });
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
