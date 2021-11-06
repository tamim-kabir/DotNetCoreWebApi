using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.Dtos
{
    public class EmployeeModelUpdateDto
    {
        [Column(TypeName = "varchar(50)")]
        public string EmpName { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Occopation { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Image { get; set; }
    }
}
