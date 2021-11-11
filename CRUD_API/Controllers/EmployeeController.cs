using AutoMapper;
using CRUD_API.Data;
using CRUD_API.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.Models
{
    [Route("api/Employee")]//[Controller]/[Action]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepo<EmployeeModel> _repo;
        private readonly IMapper _mapper;
        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="employeeRepo"></param>
        /// <param name="mapper"></param>
        public EmployeeController(IEmployeeRepo employeeRepo, IMapper mapper)
        {
            _repo = employeeRepo;
            _mapper = mapper;
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
        
        
    }
}
