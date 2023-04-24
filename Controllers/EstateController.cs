using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Net7Kubernetes.Data.interfaces;
using Net7Kubernetes.DTOs.Estate;
using Net7Kubernetes.Middleware;
using Net7Kubernetes.Models;

namespace Net7Kubernetes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstateController : ControllerBase
    {
        private readonly IEstateRepository _estateRepository;
        private readonly IMapper _mapper;

        public EstateController(IEstateRepository estateRepository, IMapper mapper)
        {
            _estateRepository = estateRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstateResponseDTO>>> GetAllEstates()
        {
            var estates = await _estateRepository.GetAllEstates();
            var estatesDTO = _mapper.Map<IEnumerable<Estate>>(estates);
             
            return Ok(estatesDTO);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<EstateResponseDTO>> GetEstateById(int id)
        {
            var estate = await _estateRepository.GetEstateById(id);
            if(estate == null)
            {
                throw new MiddlewareException(
                    HttpStatusCode.NotFound,
                    new { message = $"No se encontro un inmueble con el id: {id}"}
                );
            }

            var estateDTO = _mapper.Map<EstateResponseDTO>(estate);
            
            return Ok(estateDTO);
        }

        [HttpPost]
        public async  Task<ActionResult<EstateResponseDTO>> CreateEstate([FromBody] EstateRequestDTO requestDTO)
        {
            var estate = _mapper.Map<Estate>(requestDTO);
            await _estateRepository.CreateEstate(estate);

            await _estateRepository.SaveChanges();

            var estateResponseDTO = _mapper.Map<EstateResponseDTO>(estate);

            return CreatedAtRoute(nameof(GetEstateById), new {estateResponseDTO.Id}, estateResponseDTO);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> DeleteEstate(int id)
        {

            await _estateRepository.DeleteEstate(id);
            await _estateRepository.SaveChanges();
            
            return Ok();
        }

    }
}