using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_API.Dtos
{
    public class EmployeeModelCreateDto
    {
        [Column(TypeName = "varchar(50)")]
        public string EmpName { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Occopation { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
