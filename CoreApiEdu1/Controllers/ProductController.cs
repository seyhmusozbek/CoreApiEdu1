using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CoreApiEdu1.Entities;
using CoreApiEdu1.IRepository;
using CoreApiEdu1.Models;
using Microsoft.Extensions.Logging;

namespace CoreApiEdu1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _iLogger;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, ILogger<ProductController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _iLogger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _unitOfWork.products.GetAll();
                var results = _mapper.Map<List<ProductDTO>>(products);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetProducts)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
        
        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.products.Get(a => a.id == id);
                var result = _mapper.Map<ProductDTO>(product);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetProduct)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] AddProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                _iLogger.LogError($"Invalid post attempt in {nameof(CreateProduct)}");
                return BadRequest(ModelState);
            }
            try
            {
                var product = _mapper.Map<Product>(productDto);
                await _unitOfWork.products.Insert(product);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetProduct",new {id = product.id}, product);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex,$"Invalid post attempt in {nameof(CreateProduct)}");
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                _iLogger.LogError($"invalid update attempt in the {nameof(UpdateProduct)}");
                return BadRequest( "Submitted data is not valid.");
            }
            try
            {
                var product = await _unitOfWork.products.Get(a => a.id == id);
                if (product==null)
                {
                    _iLogger.LogError($"something went wrong in the {nameof(UpdateProduct)}");
                    return StatusCode(500, "Internal server error. Please try again later.");
                }
                _mapper.Map(productDto, product);
                _unitOfWork.products.Update(product);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(UpdateProduct)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
