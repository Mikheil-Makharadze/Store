﻿using API.DTO;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetProducer(int id)
        {
            try
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

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = mapper.Map<ProducerDTO>(producer)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetAllProducers()
        {
            try
            {
                var producers = await producerService.GetAllAsync();
                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = mapper.Map<List<ProducerDTO>>(producers)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateProducer([FromBody] ProducerCreateDTO createProducerDTO)
        {
            try
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

                return Created("Producers", new APIResponse
                {
                    StatusCode = HttpStatusCode.Created,
                    IsSuccess = true,
                    Result = mapper.Map<ProducerDTO>(producer)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }


        [Authorize(Policy = "AdminOrEmployee")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteProducer(int id)
        {
            try
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

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.NoContent,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateProducer([FromBody] ProducerUpdateDTO producerDTO)
        {
            try
            {
                var existingProducer = await producerService.GetByIdAsync(producerDTO.Id);
                if (existingProducer == null)
                {
                    return NotFound(new APIResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Producer was not found" }
                    });
                }

                if (existingProducer.Name != producerDTO.Name)
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

                mapper.Map(producerDTO, existingProducer);
                await producerService.UpdateAsync(existingProducer);

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.NoContent,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }

    }
}
