using AutoMapper;
using CRUD_API.Data;
using CRUD_API.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System;

namespace CRUD_API.Models
{
    [Route("api/[Controller]/[Action]")]//[Controller]/[Action]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepo _repo;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnv;
        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="employeeRepo"></param>
        /// <param name="mapper"></param>
        public EmployeeController(IEmployeeRepo employeeRepo, IMapper mapper, IHostingEnvironment Env)
        {
            _repo = employeeRepo;
            _mapper = mapper;
            _hostingEnv = Env;
        }
        //private readonly EmployeeRepo _repo = new EmployeeRepo();

        ///<summary>Create Employee Moedel</summary>
        ///<param name="employeeCreateDto">api/employee/{Take employee Model to new create new emp}</param>
        ///<returns>
        ///To return 201 status ok i use CreatedAtRoute function and return CreatedAtRouteResult
        ///Source-1 define: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.createdatroute?view=aspnetcore-5.0
        ///Source-2 code: https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0
        ///</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeModelReadDto>> CreateEmployee(EmployeeModelCreateDto employeeCreateDto)
        {
            var createEmp = _mapper.Map<EmployeeModel>(employeeCreateDto);
            _repo.CreateEmployee(createEmp);
            await _repo.SaveChange();
            var createReadDto = _mapper.Map<EmployeeModelReadDto>(createEmp);
            return  CreatedAtRoute(nameof(GetEmpById), new { ID = createReadDto.ID }, createReadDto);
        }
        /// <summary>
        /// Updating Employee all the data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modelUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult UpdateEmployee(int id, EmployeeModelUpdateDto modelUpdateDto)
        {
            var updateEmpRepo = _repo.GetEmployeeById(id);
            if (updateEmpRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(modelUpdateDto, updateEmpRepo);
            _repo.UpdateEmployee(updateEmpRepo);
            _repo.SaveChange();

            return NoContent();
        }

        //PATCH
        [HttpPatch("{id}")]
        public ActionResult PartialEmployeeUpdate(int id, JsonPatchDocument<EmployeeModelUpdateDto> jsonPatchDocEmpUpdate )
        {
            var empRepoIsn= _repo.GetEmployeeById(id);
            if(empRepoIsn == null)
            {
                return NotFound();
            }
            var mapToModelPath = _mapper.Map<EmployeeModelUpdateDto>(empRepoIsn);
            jsonPatchDocEmpUpdate.ApplyTo(mapToModelPath, ModelState);

            if(!TryValidateModel(mapToModelPath))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(mapToModelPath, empRepoIsn);
            _repo.UpdateEmployee(empRepoIsn);
            _repo.SaveChange();

            return NoContent();
        }

        public ActionResult DeleteEmployee(int id)
        {
            var empRepoIsn = _repo.GetEmployeeById(id);
            if (empRepoIsn == null)
            {
                return NotFound();
            }
            _repo.DeleteEmployee(empRepoIsn);
            _repo.SaveChange();

            return NoContent();
        }
        ///<summary>Get Employee by thare id : api/employee/</summary>
        ///<returns>If found return single object or return 404 Not Found</returns>
        [HttpGet]
        public ActionResult<IEnumerable<EmployeeModelReadDto>> GetAllEmp()
        {
            var Emp = _repo.GetAllEmployee();
            if(Emp != null)
            {
                return Ok(_mapper.Map<IEnumerable<EmployeeModelReadDto>>(Emp));
            }
            return NotFound();
        }

        ///<summary>Get Employee by thare id</summary>
        ///<param name="id">api/employee/{id}</param>
        ///<returns>If found return single object or return 404 Not Found</returns>
        [HttpGet("{id}", Name= "GetEmpById")]
        public ActionResult<EmployeeModelReadDto> GetEmpById(int id)
        {
            var emp = _repo.GetEmployeeById(id);
            if (emp != null)
            {
                return Ok(_mapper.Map<EmployeeModelReadDto>(emp));
            }
            return NotFound();
        }
        
        [HttpPost("id")]
        public async Task<string> UploadEmpImage(int id)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                var emp = _repo.GetEmployeeById(id);
                if(files != null && files.Count == 1)
                {
                    
                    foreach(var file in files)
                    {
                        emp.Image = await UploadImage(file);
                    }
                }
                _repo.UpdateEmployee(emp);
                await _repo.SaveChange();
            }
            catch(Exception e)
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
            var imgPath = Path.Combine(_hostingEnv.ContentRootPath, "Images", imageName);

            using (var fileStream = new FileStream(imgPath, FileMode.Create))
            {
                await imgFile.CopyToAsync(fileStream);
            }
            return imageName;
        }
    }
}
