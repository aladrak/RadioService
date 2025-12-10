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
		{
			public int CompanyID { get; set; }
			public int PersonID { get; set; }
			public string Bank { get; set; }
			public string AccountNum { get; set; }
			public string ITN { get; set; }
		}

		public class Product
		{
			public int ProductID { get; set; }
			public string Mark { get; set; }
			public string Country { get; set; }
			public string Manufacturer { get; set; }
			public string Photo { get; set; }
			public DateTime ServiceLife { get; set; }
		}

		public class Specialty
		{
			public int SpecialtyID { get; set; }
			public string Name { get; set; }
			public string Description { get; set; }
		}

		public class Employee
		{
			public int EmployeeID { get; set; }
			public int SpecialtyID { get; set; }
			public string FirstName { get; set; }
			public string MidName { get; set; }
			public string LastName { get; set; }
			public string Address { get; set; }
			public string Phone { get; set; }
			public int Age { get; set; }
			public int Skill { get; set; }
		}

		public class Order
		{
			public int OrderID { get; set; }
			public int ProductID { get; set; }
			public int CompanyID { get; set; }
			public int EmployeeID { get; set; }
			public DateTime StartDate { get; set; }
			public DateTime FinishDate { get; set; }
			public List<string> FaultsList { get; set; }
			public double Price { get; set; }
		}
	}
}
