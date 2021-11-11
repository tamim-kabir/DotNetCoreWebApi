using CRUD_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.Data
{
    public interface IEmployeeRepo<T> where T : class, IApplicatioRepo<T> 
    {

        IEnumerable<EmployeeModel> GetAllEmployee();
        EmployeeModel GetEmployeeById(int id);
        void CreateEmployee(EmployeeModel employee);
        void UpdateEmployee(EmployeeModel employeeModel);
        void DeleteEmployee(EmployeeModel employee);
    }
}
