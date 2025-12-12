namespace Radiotech.Data
{
	public static class TableData
	{
		public class Person 
		{
			public int PersonID { get; init; }
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
			public int CompanyID { get; init; }
			public int? PersonID { get; set; }
			public string Bank { get; set; }
			public string AccountNum { get; set; } // 20 digits
			public string ITN { get; set; } // ИНН для организаций 10 для физ лиц 12
		}

		public class Product
		{
			public int ProductID { get; init; }
			public string Mark { get; set; }
			public string Country { get; set; }
			public string Manufacturer { get; set; }
			public string Photo { get; set; } // link to photo
			public DateTime ServiceLife { get; set; }
		}

		public class Specialty
		{
			public int SpecialtyID { get; init; }
			public string Name { get; set; }
			public string Description { get; set; }
		}

		public class Employee
		{
			public int EmployeeID { get; init; }
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
			public int OrderID { get; init; }
			public int ProductID { get; set; }
			public int CompanyID { get; set; }
			public int EmployeeID { get; set; }
			public DateTime StartDate { get; set; } // DateTime field
			public DateTime FinishDate { get; set; }
			public List<string> FaultsList { get; set; } // thinking...
			public double Price { get; set; }
		}
	}
}
