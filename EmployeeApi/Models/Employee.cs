namespace EmployeeApi.Models
{
	public class Employee
	{
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? Department { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
