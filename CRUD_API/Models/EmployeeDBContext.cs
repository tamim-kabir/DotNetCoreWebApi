using Microsoft.EntityFrameworkCore;

namespace CRUD_API.Models
{
    public class EmployeeDBContext:DbContext
    {
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> option):base(option)
        {
        }
        public DbSet<EmployeeModel> Employees { get; set; }
    }
}
