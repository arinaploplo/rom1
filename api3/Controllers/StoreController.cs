using api3.Interface;
using api3.Models;
using api3.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using api3.Repository;

namespace api3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController : Controller
    {
        private readonly InterfaceStore _RepositoryStore;
        private readonly IMapper _mapper;

        public StoreController(InterfaceStore RepositoryStore,  IMapper mapper)
        {
            _RepositoryStore = RepositoryStore;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Store>))]
        public IActionResult GetStore()
        {
            var Store = _mapper.Map<List<StoreDto>>(_RepositoryStore.GetStore());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Store);
        }



 

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] StoreDto StoreDTO)
        {
            // por si el DTO es null
            if (StoreDTO == null || !ModelState.IsValid) { return BadRequest(ModelState); }
            if (_RepositoryStore.StoreExist(StoreDTO.IdStore))
            {
                return StatusCode(666, "Store ya existe");
            }

            var Store = _mapper.Map<Store>(StoreDTO);

            if (!_RepositoryStore.CreateStore(Store))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            else
            {
                return Ok("Se ha registrado");
            }

        }


        [HttpPut("{StoreId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStore(int StoreId, [FromBody] StoreUpdateDto updatedStore)
        {
            if (updatedStore == null)
                return BadRequest(ModelState);

            if (!_RepositoryStore.StoreExist(StoreId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var StoreMap = _mapper.Map<Store>(updatedStore);
            StoreMap.IdStore = StoreId;
            if (!_RepositoryStore.UpdateStore(StoreId, StoreMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            } 

            return Ok("Se ha actualizado con exito");
        }
        
        [HttpDelete("{StoreId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteStore(int StoreId)
        {
            if (!_RepositoryStore.StoreExist(StoreId))
            {
                return NotFound();
            }

            var StoreToDelete = _RepositoryStore.GetStore(StoreId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_RepositoryStore.DeleteStore(StoreToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return Ok("Se ha eliminado la tabla");
        }
        
    }
}