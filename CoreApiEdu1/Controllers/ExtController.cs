using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AutoMapper;
using CoreApiEdu1.ADONet.AdoOps;
using CoreApiEdu1.Entities;
using CoreApiEdu1.IRepository;
using CoreApiEdu1.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreApiEdu1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExtController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExtController> _iLogger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ExtruderOps _extruderOps;

        public ExtController(IUnitOfWork unitOfWork, ILogger<ExtController> logger, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _iLogger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _extruderOps = new ExtruderOps(_configuration);
        }

        [HttpGet("{machine}")]
        public async Task<IActionResult> GetWorkOrders(string machine)
        {
            try
            {
                var workOrders = await _extruderOps.GetExtOrders(machine);
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetWorkOrders)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("{id:int}", Name = "GetProduction")]
        public async Task<IActionResult> GetProduction(int id)
        {
            try
            {
                var production = await _unitOfWork.productions.Get(a => a.id == id);
                return Ok(production);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetProduction)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
        [HttpGet("{orderId:int}", Name = "GetProductions")]
        public async Task<IActionResult> GetProductions(int orderId)
        {
            try
            {
                var production = await _unitOfWork.productions.GetAll(a => a.orderId == orderId);
                return Ok(production);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetProductions)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProduction([FromBody] AddProductionDTO addProductionDTO)
        {
            if (!ModelState.IsValid)
            {
                _iLogger.LogError($"Invalid post attempt in {nameof(AddProduction)}");
                return BadRequest(ModelState);
            }
            try
            {
                var production = _mapper.Map<Production>(addProductionDTO);
                production.saveDate = DateTime.Now;
                await _unitOfWork.productions.Insert(production);
                await _unitOfWork.Save();
                return Ok(new {prod=production, success=true});
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid post attempt in {nameof(AddProduction)}");
                return BadRequest(new {success=false, product = addProductionDTO});
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduction(int id)
        {
            if (id < 1)
            {
                _iLogger.LogError($"Invalid delete attempt in {nameof(DeleteProduction)}");
                return BadRequest(ModelState);
            }
            try
            {
                var production = await _unitOfWork.productions.Get(a => a.id == id);
                if (production == null)
                {
                    _iLogger.LogError($"Invalid delete attempt in {nameof(DeleteProduction)}");
                    return BadRequest("There is nothing to delete.");
                }

                await _unitOfWork.productions.Delete(id);
                await _unitOfWork.Save();

                return Ok(new{success=true});
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid delete attempt in {nameof(DeleteProduction)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
