using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_API.Models
{
    public class EmployeeImgModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Column(TypeName ="VARCHAR(50)")]
        public string Images { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
