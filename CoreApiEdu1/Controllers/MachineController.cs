using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AutoMapper;
using CoreApiEdu1.Entities;
using CoreApiEdu1.IRepository;
using CoreApiEdu1.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace CoreApiEdu1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExtController> _iLogger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public MachineController(IUnitOfWork unitOfWork, ILogger<ExtController> logger, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _iLogger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet("{group}")]
        public async Task<IActionResult> GetMachines(string group)
        {
            try
            {
                var machines = await _unitOfWork.machines.GetAll(a=> a.machineGroup== group);
                return Ok(machines);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetMachines)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
