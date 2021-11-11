using CRUD_API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.Controllers
{
    [Route("/api/ImagesUpload")]
    public class ImagesUploadControllers : ControllerBase  
    {
        private readonly IEmployeeRepo _repo;
        private readonly IWebHostEnvironment _hostEnv;

        public ImagesUploadControllers(IEmployeeRepo employeeRepo, IWebHostEnvironment webHost)
        {
            _repo = employeeRepo;
            _hostEnv = webHost; 
        }
        [HttpPost("id")]
        [Route("/UploadEmpImage")]
        public async Task<string> UploadEmpImage(int id)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                var emp = _repo.GetEmployeeById(id);
                if (files != null && files.Count == 1)
                {

                    foreach (var file in files)
                    {
                        emp.Image = await UploadImage(file);
                    }
                }
                _repo.UpdateEmployee(emp);
                await _repo.SaveChange();
            }
            catch (Exception e)
            {
                throw e;
            }
            return "Employee images saved";
        }

        [NonAction]
        public async Task<string> UploadImage(IFormFile imgFile)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imgFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName += DateTime.Now.ToString("yymmddhhssff") + Path.GetExtension(imgFile.FileName);
            var imgPath = Path.Combine(_hostEnv.ContentRootPath, "Images", imageName);

            using (var fileStream = new FileStream(imgPath, FileMode.Create))
            {
                await imgFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

    }
}
