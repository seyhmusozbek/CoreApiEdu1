using AutoMapper;
using CoreApiEdu1.ADONet.AdoOps;
using CoreApiEdu1.Entities;
using CoreApiEdu1.IRepository;
using CoreApiEdu1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WMSController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExtController> _iLogger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ExtruderOps _extruderOps;
        private readonly WMSOps _wmsops;


        public WMSController(IUnitOfWork unitOfWork, ILogger<ExtController> logger, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _iLogger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _extruderOps = new ExtruderOps(_configuration);
            _wmsops = new WMSOps(_configuration);
        }
        [Authorize]
        [HttpGet("{code}", Name = "GetTransactions")]
        public async Task<IActionResult> GetTransactions(string code)
        {
            try
            {
                var transactions = await _extruderOps.GetTransactions(code);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetTransactions)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddWHTransfer([FromBody] List<AddWHTransferDTO> addWHTransferDTO)
        {
            if (!ModelState.IsValid)
            {
                _iLogger.LogError($"Invalid post attempt in {nameof(AddWHTransfer)}");
                return BadRequest(ModelState);
            }
            try
            {
                var wht = _mapper.Map<List<WHTransfer>>(addWHTransferDTO);
                if (wht.Count > 0)
                {
                    await _unitOfWork.wHTransfer.InsertRange(wht);
                    await _unitOfWork.Save();
                    await _wmsops.WMSInsertToERP(wht);
                    return Ok(new { sReserve = wht, success = true });
                }
                else
                {
                return BadRequest(new { success = false, sReserve = addWHTransferDTO });

                }

            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid post attempt in {nameof(AddWHTransfer)}");
                return BadRequest(new { success = false, sReserve = addWHTransferDTO });
            }
        }
    }
}
