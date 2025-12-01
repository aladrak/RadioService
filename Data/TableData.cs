
namespace Radiotech.Data
{
	public static class TableData
	{
		public class Person(
			int personID, 
			string firstName, 
			string midName, 
			string lastName, 
			string address, 
			string phone ) 
		{
			public int PersonID { get; set; } = personID;
			public string FirstName { get; set; } = firstName;
			public string MidName { get; set; } = midName;
			public string LastName { get; set; } = lastName;
			public string Address { get; set; } = address;
			public string Phone { get; set; } = phone;

			public override string ToString()
			{
				return $"{PersonID} {FirstName} {MidName} {LastName} {Address} {Phone}";
			}

			public string Path { get; } = "Person.json";
		}
	}
}
