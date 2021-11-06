using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.Dtos
{
    public class EmployeeModelReadDto
    {
        public int ID { get; set; }
        public string EmpName { get; set; }
        public string Occopation { get; set; }
        public string Image { get; set; }
    }
}
