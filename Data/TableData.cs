
namespace Radiotech.Data
{
	public static class TableData
	{
		public class Person 
		{
			public int PersonID { get; set; }
			public string FirstName { get; set; }
			public string MidName { get; set; }
			public string LastName { get; set; }
			public string Address { get; set; }
			public string Phone { get; set; }

			public override string ToString()
			{
				return $"{PersonID} {FirstName} {MidName} {LastName} {Address} {Phone}";
			}
		}
		public class Company
		(
			int CompanyID,
			int PersonID,
			string Bank,
			string AccountNum,
			string ITN
		);

		public class Product
		(
			int ProductID,
			string Mark,
			string Country,
			string Manufacturer,
			string Photo,
			DateTime ServiceLife
		);

		public class Specialty
		(
			int SpecialtyID,
			string Name,
			string Description
		);

		public class Employee
		(
			int EmployeeID,
			int SpecialtyID,
			string FirstName,
			string MidName,
			string LastName,
			string Address,
			string Phone,
			int Age,
			int Skill
		);

		public class Order
		(
			int OrderID,
			int ProductID,
			int CompanyID,
			int EmployeeID,
			DateTime StartDate,
			DateTime FinishDate,
			List<string> FaultsList,
			double Price
		);
	}
}
