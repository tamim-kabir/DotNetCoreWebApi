using CRUD_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.Data
{
    public class SqlEmployeeRepo : IEmployeeRepo
    {
        private readonly EmployeeDBContext _Context;

        public SqlEmployeeRepo(EmployeeDBContext context)
        {
            _Context = context;
        }

        public void CreateEmployee(EmployeeModel employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));                
            }
            _Context.Employees.Add(employee);
        }

        public IEnumerable<EmployeeModel> GetAllEmployee()
            => _Context.Employees.ToList();


        public EmployeeModel GetEmployeeById(int id)
            => _Context.Employees.FirstOrDefault(m => m.ID == id);

        public bool SaveChange() 
            => _Context.SaveChanges() >= 0;

        public void UpdateEmployee(EmployeeModel employeeModel)
        {
            ///<summary>
            ///Not Need Implement any thing
            ///</summary>
        }
        public void DeleteEmployee(EmployeeModel employee)
        {
            if(employee == null)
            {
                throw new ArgumentNullException(nameof(employee));

            }
            _Context.Employees.Remove(employee);
        }
    }
}
