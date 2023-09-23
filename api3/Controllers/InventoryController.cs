using api3.Interface;
using api3.Models;
using api3.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper;

namespace api3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : Controller
    {
        private readonly InterfaceInventory _RepositoryInventory;
        private readonly InterfaceStore _RepositoryStore;
        private readonly InterfaceEmployee _RepositoryEmployee;
        private readonly IMapper _mapper;

        public InventoryController(InterfaceInventory RepositoryInventory, InterfaceStore RepositoryStore, InterfaceEmployee RepositoryEmployee, IMapper mapper)
        {
            _RepositoryInventory = RepositoryInventory;
            _RepositoryStore = RepositoryStore;
            _RepositoryEmployee = RepositoryEmployee;
            _mapper = mapper;
        }
        [HttpGet("/inventory")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Inventory>))]
        public IActionResult GetInventory()
        {
            var Inventory = _mapper.Map<List<InventoryDto>>(_RepositoryInventory.GetInventory());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Inventory);
        }

        [HttpGet("/inventory/{InventoryId}")]
        [ProducesResponseType(200, Type = typeof(InventoryDto))]

        public IActionResult Get(int InventoryId)
        {
            var Inventory = _mapper.Map<InventoryDto>(_RepositoryInventory.GetInventory(InventoryId));
            if (Inventory == null || !ModelState.IsValid)
            {
                ModelState.AddModelError("", "No se puedo encontro la habitación");
                return StatusCode(404, ModelState);
            }
            return Ok(Inventory);
        }





        [HttpPost("/inventory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] InventoryDto InventoryDTO)
        {
            // por si el DTO es null
            if (InventoryDTO == null || !ModelState.IsValid) { return BadRequest(ModelState); }

            // por si las tablas de las llaves forraneas no existen
            if (!_RepositoryStore.StoreExist(InventoryDTO.IdStore)) { return NotFound("No se encontro la tabla del IdStore que proporcionaste"); }

            if (!_RepositoryEmployee.EmployeeExist(InventoryDTO.IdEmployee)) { return NotFound("No se encontro la tabla del IdEmployee que proporcionaste"); }


            


            var Inventory = _mapper.Map<Inventory>(InventoryDTO);


            if (!_RepositoryInventory.CreateInventory(Inventory))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            else
            {
                return Ok("Se ha registrado");
            }

        }


        [HttpPut("/inventory/{InventoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateInventory(int InventoryId, [FromBody] InventoryUpdateDto updatedInventory)
        {
            if (updatedInventory == null)
                return BadRequest(ModelState);
            if (!_RepositoryInventory.InventoryExist(InventoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            int employeeId = _RepositoryEmployee.GetEmployeeIdByName(updatedInventory.ListedBy);
            //if (!_RepositoryStore.StoreExist(storeId)) { return NotFound("No se encontro la tabla del IdStore que proporcionaste"); }

             if (!_RepositoryEmployee.EmployeeExist(employeeId)) { return NotFound("No se encontro la tabla del IdEmployee que proporcionaste"); }
            var filterUpdate = _RepositoryInventory.GetInventory(InventoryId); // Obtener los datos para que solo actualice lo que quiero

            var InventoryMap = _mapper.Map<Inventory>(filterUpdate);
            InventoryMap.Quantity = updatedInventory.Quantity; // Actualizar quantity
            InventoryMap.IdEmployee = employeeId; // Actualizar id de store

            if (!_RepositoryInventory.UpdateInventory(InventoryId, InventoryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Se ha actualizado con exito");
        }

        [HttpDelete("/inventory/{InventoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteInventory(int InventoryId)
        {
            if (!_RepositoryInventory.InventoryExist(InventoryId))
            {
                return NotFound();
            }

            var InventoryToDelete = _RepositoryInventory.GetInventory(InventoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_RepositoryInventory.DeleteInventory(InventoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return Ok("Se ha eliminado la tabla");
        }
        [HttpPost("/inventory/upload")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult UploadInventory([FromForm] IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError("", "No se proporcionó ningún archivo CSV.");
                return BadRequest(ModelState);
            }

            try
            {
                using (var streamReader = new StreamReader(csvFile.OpenReadStream()))
                    
                using (var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Configurar el separador de campo
                    Delimiter = ","
                }))

                {

                    var records = csvReader.GetRecords<InventoryCsvDto>()
                        .Where(record => !string.IsNullOrWhiteSpace(record.Store)) // Eliminar espacios en blanco
                        .ToList(); // lista de registros





                    foreach (var record in records)
                    {
                        StoreDto StoreDTO = new StoreDto(); // Crear una nueva instancia
                        EmployeeDto EmployeeDTO = new EmployeeDto(); // Crear una nueva instancia
                        InventoryDto InventoryDTO = new InventoryDto(); // Crear una nueva instancia

                        // Agregar el store
                        int storeId = _RepositoryStore.GetStoreIdByName(record.Store.Trim());
                        
                        if (storeId == -1) {
                            StoreDTO.IdStore = 0;
                            StoreDTO.IdStore = _RepositoryStore.GetNextStoreId();

                            if (_RepositoryStore.StoreExist(StoreDTO.IdStore))
                            {
                                return StatusCode(666, "Store ya existe");
                            }

                            StoreDTO.Name = record.Store.Trim();

                            var Store = _mapper.Map<Store>(StoreDTO);

                            if (!_RepositoryStore.CreateStore(Store))
                            {
                                ModelState.AddModelError("", "Something went wrong while saving");
                                return StatusCode(500, ModelState);
                            }
                        } else { StoreDTO.IdStore = storeId; }
                        

                      

                        //Agregar el empleado
                        int EmployeeId = _RepositoryEmployee.GetEmployeeIdByName(record.ListedBy.Trim());

                        if (EmployeeId == -1)
                        {
                            EmployeeDTO.IdEmployee = 0;
                            EmployeeDTO.IdEmployee = _RepositoryEmployee.GetNextEmployeeId();

                            if (_RepositoryEmployee.EmployeeExist(EmployeeDTO.IdEmployee))
                            {
                                return StatusCode(666, "Employee ya existe");
                            }

                            EmployeeDTO.Name = record.ListedBy.Trim();

                            var Employee = _mapper.Map<Employee>(EmployeeDTO);

                            if (!_RepositoryEmployee.CreateEmployee(Employee))
                            {
                                ModelState.AddModelError("", "Something went wrong while saving");
                                return StatusCode(500, ModelState);
                            }
                        }
                        else { EmployeeDTO.IdEmployee = EmployeeId; }
                        
                       
                        


                        InventoryDTO.IdInventory = _RepositoryInventory.GetNextInventoryId();
                        InventoryDTO.IdEmployee = EmployeeDTO.IdEmployee;
                        InventoryDTO.IdStore = StoreDTO.IdStore;
                        //InventoryDTO.Date = Convert.ToDateTime(record.Date);
                        InventoryDTO.Date = DateTime.SpecifyKind(Convert.ToDateTime(record.Date), DateTimeKind.Utc); // Por el tema del timeswamp
                        InventoryDTO.Flavor = record.Flavor;
                        InventoryDTO.IsSeasonFlavor = record.IsSeasonFlavor;
                        InventoryDTO.Quantity = Convert.ToInt32(record.Quantity);


                        var Inventory = _mapper.Map<Inventory>(InventoryDTO);


                        if (!_RepositoryInventory.CreateInventory(Inventory))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving");
                            return StatusCode(500, ModelState);
                        }
                    }
                }

                return StatusCode(201); // Carga exitosa
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al procesar el archivo CSV: " + ex.Message);

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("InnerException", ex.InnerException.Message);
                }

                return BadRequest(ModelState);
            }
        }
    }
}