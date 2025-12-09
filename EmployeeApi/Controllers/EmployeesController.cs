using EmployeeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmployeeApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EmployeesController : ControllerBase
	{
		private static readonly object _fileLock = new();
		private readonly string _dataFilePath;

		public EmployeesController(IWebHostEnvironment env)
		{
			var dataFolder = Path.Combine(env.ContentRootPath, "Data");
			if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);

			_dataFilePath = Path.Combine(dataFolder, "employees.json");
		}

		// POST /api/employees
		[HttpPost]
		public IActionResult CreateEmployee([FromBody] Employee employee)
		{
			if (employee == null) return BadRequest("Employee is required.");
			if (string.IsNullOrWhiteSpace(employee.Name)) return BadRequest("Name is required.");
			if (string.IsNullOrWhiteSpace(employee.Email)) return BadRequest("Email is required.");

			List<Employee> list;

			lock (_fileLock)
			{
				if (System.IO.File.Exists(_dataFilePath))
				{
					var json = System.IO.File.ReadAllText(_dataFilePath);
					list = string.IsNullOrWhiteSpace(json)
						? new List<Employee>()
						: JsonSerializer.Deserialize<List<Employee>>(json) ?? new List<Employee>();
				}
				else
				{
					list = new List<Employee>();
				}

				employee.CreatedAt = DateTime.UtcNow;
				list.Add(employee);

				var newJson = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
				System.IO.File.WriteAllText(_dataFilePath, newJson);
			}

			return CreatedAtAction(nameof(GetAll), null, employee);
		}

		// GET /api/employees
		[HttpGet]
		public IActionResult GetAll()
		{
			List<Employee> list;

			lock (_fileLock)
			{
				if (!System.IO.File.Exists(_dataFilePath))
				{
					return Ok(new List<Employee>());
				}

				var json = System.IO.File.ReadAllText(_dataFilePath);
				list = string.IsNullOrWhiteSpace(json)
					? new List<Employee>()
					: JsonSerializer.Deserialize<List<Employee>>(json) ?? new List<Employee>();
			}

			return Ok(list);
		}
	}
}
