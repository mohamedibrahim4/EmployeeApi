# EmployeeApi

Simple ASP.NET Core Web API using .NET 7.  
Stores employee records in a JSON file (no database).

## Endpoints

### POST /api/employees
Adds a new employee.

Body example:
```json
{
    "Name": "Mohamed Ibrahim Abdelkader",
    "Email": "mohamedatlm297@gmail.com",
    "Phone": "0522702269",
    "Department": "CS",
    "CreatedAt": "2025-12-09T18:50:38.4492749Z"
  }
