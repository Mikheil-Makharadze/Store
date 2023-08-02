using API.DTO;
using API.Response;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Producer CRUD
    /// </summary>
    public class ProducerController : BaseApiController
    {
        private readonly IProducerService producerService;
        private readonly IMapper mapper;

        /// <summary>
        /// Injectint Services
        /// </summary>
        /// <param name="_producerService"></param>
        /// <param name="_mapper"></param>
        public ProducerController(IProducerService _producerService, IMapper _mapper)
        {
            producerService = _producerService;
            mapper = _mapper;
        }

        /// <summary>
        /// Get Producer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProducer(int id)
        {
            return Ok(new APIResponse { Result = mapper.Map<ProducerDTO>(await producerService.GetByIdAsync(id, n => n.Products)) });
        }

        /// <summary>
        /// Get All Producers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllProducers()
        {
            return Ok(new APIResponse { Result = mapper.Map<List<ProducerDTO>>(await producerService.GetAllAsync()) });
        }

        /// <summary>
        /// Get All Producers Details
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpGet("AllDetails")]
        public async Task<ActionResult<APIResponse>> GetAllProducersDetails(SearchString searchString)
        {
            return Ok(new APIResponse { Result = mapper.Map<List<ProducerDTO>>(await producerService.GetAllProducersDetails(searchString.Search)) });
        }

        /// <summary>
        /// Create New Producer
        /// </summary>
        /// <param name="producerDTO"></param>
        /// <returns></returns>
        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateProducer([FromBody] ProducerCreateDTO producerDTO)
        {
            return Ok(new APIResponse { Result = mapper.Map<ProducerDTO>(await producerService.AddAsync(mapper.Map<Producer>(producerDTO))) });
        }

        /// <summary>
        /// Delete Producer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "AdminOrEmployee")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteProducer(int id)
        {
            return Ok(new APIResponse { Result = await producerService.DeleteAsync(id) });
        }

        /// <summary>
        /// Update Producer
        /// </summary>
        /// <param name="producerDTO"></param>
        /// <returns></returns>
        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateProducer([FromBody] ProducerUpdateDTO producerDTO)
        {
            return Ok(new APIResponse { Result = await producerService.UpdateAsync(mapper.Map<Producer>(producerDTO)) });
        }
    }
}
