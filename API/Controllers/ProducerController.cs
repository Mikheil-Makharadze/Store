using API.DTO;
using API.Response;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    public class ProducerController : BaseApiController
    {
        private readonly IProducerService producerService;
        private readonly IMapper mapper;
        public ProducerController(IProducerService _producerService, IMapper _mapper)
        {
            producerService = _producerService;
            mapper = _mapper;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProducer(int id)
        {
            var producer = await producerService.GetByIdAsync(id, n => n.Products);

            if (producer == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Producer was not found" }
                });
            }

            return Ok(new APIResponse { Result = mapper.Map<ProducerDTO>(producer) });
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllProducers()
        {
            var producers = await producerService.GetAllAsync();

            return Ok(new APIResponse { Result = mapper.Map<List<ProducerDTO>>(producers) });
        }

        [HttpGet("AllDetails")]
        public async Task<ActionResult<APIResponse>> GetAllProducersDetails(SearchString searchString)
        {
            var producers = await producerService.GetAllAsync(n => n.Products);

            if (searchString.Search != null)
            {
                var search = searchString.Search.Trim();
                producers = producers.Where(n => n.Name.Contains(search) || n.Description.Contains(search)).ToList();
            }

            return Ok(new APIResponse { Result = mapper.Map<List<ProducerDTO>>(producers) });
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateProducer([FromBody] ProducerCreateDTO createProducerDTO)
        {
            var producers = await producerService.GetAllAsync();
            if (producers.Any(n => n.Name.ToLower() == createProducerDTO.Name.ToLower()))
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Producer with same name already exists" }
                });
            }

            var producer = mapper.Map<Producer>(createProducerDTO);
            await producerService.AddAsync(producer);

            return Ok(new APIResponse { Result = mapper.Map<ProducerDTO>(producer) });
        }


        [Authorize(Policy = "AdminOrEmployee")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteProducer(int id)
        {
            var producer = await producerService.GetByIdAsync(id);
            if (producer == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Producer was not found" }
                });
            }

            await producerService.DeleteAsync(producer);

            return Ok(new APIResponse { Result = producer.Id});
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateProducer([FromBody] ProducerUpdateDTO producerDTO)
        {
            var Producer = await producerService.GetByIdAsync(producerDTO.Id);
            if (Producer == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Producer was not found" }
                });
            }

            if (Producer.Name != producerDTO.Name)
            {
                var products = await producerService.GetAllAsync();
                if (products.Any(n => n.Name.ToLower() == producerDTO.Name.ToLower()))
                {
                    return BadRequest(new APIResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Producer with same name already exists" }
                    });
                }
            }

            mapper.Map(producerDTO, Producer);
            await producerService.UpdateAsync(Producer);

            return Ok(new APIResponse { Result = Producer });
        }
    }
}
