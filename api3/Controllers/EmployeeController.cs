using api3.Interface;
using api3.Models;
using api3.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly InterfaceEmployee _RepositoryEmployee;
        private readonly IMapper _mapper;

        public EmployeeController(InterfaceEmployee RepositoryEmployee,  IMapper mapper)
        {
            _RepositoryEmployee = RepositoryEmployee;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        public IActionResult GetEmployee()
        {
            var Employee = _mapper.Map<List<EmployeeDto>>(_RepositoryEmployee.GetEmployee());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Employee);
        }



 

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] EmployeeDto EmployeeDTO)
        {
            // por si el DTO es null
            if (EmployeeDTO == null || !ModelState.IsValid) { return BadRequest(ModelState); }

            if (_RepositoryEmployee.EmployeeExist(EmployeeDTO.IdEmployee))
            {
                return StatusCode(666, "Empleado ya existe");
            }

            var Employee = _mapper.Map<Employee>(EmployeeDTO);

            if (!_RepositoryEmployee.CreateEmployee(Employee))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            else
            {
                return Ok("Se ha registrado");
            }

        }


        [HttpPut("{employeeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateEmployee(int employeeId, [FromBody] EmployeeUpdateDto updatedEmployee)
        {
            if (updatedEmployee == null)
                return BadRequest(ModelState);
            
            if (!_RepositoryEmployee.EmployeeExist(employeeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var EmployeeMap = _mapper.Map<Employee>(updatedEmployee);
            EmployeeMap.IdEmployee = employeeId;
            if (!_RepositoryEmployee.UpdateEmployee(employeeId, EmployeeMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            } 

            return Ok("Se ha actualizado con exito");
        }
        
        [HttpDelete("{employeeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteEmployee(int employeeId)
        {
            if (!_RepositoryEmployee.EmployeeExist(employeeId))
            {
                return NotFound();
            }

            var employeeToDelete = _RepositoryEmployee.GetEmployee(employeeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_RepositoryEmployee.DeleteEmployee(employeeToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return Ok("Se ha eliminado la tabla");
        }
        
    }
}