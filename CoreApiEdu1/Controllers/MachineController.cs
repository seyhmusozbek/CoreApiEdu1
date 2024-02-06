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

        [HttpGet("{machine}")]
        public async Task<IActionResult> GetMStops(string machine)
        {
            try
            {
                DateTime last2Day = DateTime.Now.AddDays(-2);
                var mStops = await _unitOfWork.mStop.GetAll(a => a.machine.machineName == machine && a.date>last2Day);
                return Ok(mStops);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetMStops)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StopMachine([FromBody] AddStopDTO addStopDTO)
        {
            if (!ModelState.IsValid)
            {
                _iLogger.LogError($"Invalid post attempt in {nameof(StopMachine)}");
                return BadRequest(ModelState);
            }
            try
            {
                var stop = _mapper.Map<MStop>(addStopDTO);
                var machine = addStopDTO.machine;
                machine.currentStatus = !machine.currentStatus;
                machine.lastStopped = stop.date;
                _unitOfWork.machines.Update(machine);
                await _unitOfWork.mStop.Insert(stop);
                await _unitOfWork.Save();
                return Ok(new { stop = stop, success = true });
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid post attempt in {nameof(StopMachine)}");
                return BadRequest(new { success = false, stop = addStopDTO });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetCurrCaliber([FromBody] CaliberChange caliber)
        {
            if (!ModelState.IsValid)
            {
                _iLogger.LogError($"Invalid post attempt in {nameof(SetCurrCaliber)}");
                return BadRequest(ModelState);
            }
            try
            {
                var machine = await _unitOfWork.machines.Get(a=> a.id == caliber.id);
                if(machine!=null)
                {
                    machine.currentCaliber = caliber.caliber;
                    _unitOfWork.machines.Update(machine);
                    await _unitOfWork.Save();
                }
                return Ok(new { caliber=caliber, machine=machine });
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid post attempt in {nameof(SetCurrCaliber)}");
                return BadRequest(new { caliber = caliber });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] Machine machineRequest)
        {
            try
            {
                var machine = await _unitOfWork.machines.Get(a => a.machineName == machineRequest.machineName);
                if (machine != null)
                {
                    return BadRequest();
                }
                await _unitOfWork.machines.Insert(machineRequest);
                await _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid post attempt in {nameof(Create)}");
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] Machine machineRequest)
        {
            try
            {
                var machine = await _unitOfWork.machines.Get(a => a.machineName == machineRequest.machineName && a.machineGroup == machineRequest.machineGroup);
                if (machine == null)
                {
                    return BadRequest();
                }
                await _unitOfWork.machines.Delete(machine.id);
                await _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid post attempt in {nameof(Create)}");
                return BadRequest();
            }
        }
    }
}
