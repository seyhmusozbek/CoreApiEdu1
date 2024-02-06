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
using CoreApiEdu1.ADONet.AdoEnts;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        private const string PLANSAVEPATH = "Plans";

        public ExtController(IUnitOfWork unitOfWork, ILogger<ExtController> logger, IMapper mapper, IConfiguration configuration, ExtruderOps extruderOps)
        {
            _unitOfWork = unitOfWork;
            _iLogger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _extruderOps = extruderOps;
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

        [HttpGet("{barcodeId:int}", Name = "GetBarcodeInfo")]
        public async Task<IActionResult> GetBarcodeInfo(int barcodeId)
        {
            try
            {
                var barcodeInfo = await _extruderOps.GetBarcodeInfo(barcodeId);
                return Ok(barcodeInfo);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetBarcodeInfo)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet(Name = "GetStockInfos")]
        public async Task<IActionResult> GetStockInfos()
        {
            try
            {
                _iLogger.LogInformation("Getstockinfos called");
                var stockInfos = await _extruderOps.GetStockInfos();
                return Ok(stockInfos);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetStockInfos)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("{orderNum}", Name = "GetOrderStockInfos")]
        public async Task<IActionResult> GetOrderStockInfos(string orderNum)
        {
            try
            {
                _iLogger.LogInformation("GetOrderStockInfos called");
                var stockInfos = await _extruderOps.GetOrderStockInfos(orderNum);
                return Ok(stockInfos);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetOrderStockInfos)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet(Name = "GetCalibers")]
        public async Task<IActionResult> GetCalibers()
        {
            try
            {
                var calibers = await _extruderOps.GetCalibers();
                return Ok(calibers);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetCalibers)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePlanResult()
        {
            var bodyStr = "";
            var req = HttpContext.Request;

            req.EnableBuffering();

            using StreamReader reader
                      = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true);
            bodyStr = await reader.ReadToEndAsync();

            // Rewind, so the core is not lost when it looks at the body for the request
            req.Body.Position = 0;
            await CreateAndStoreInTextFile(bodyStr);

            return Ok();
        }

        private async Task CreateAndStoreInTextFile(string bodyStr)
        {
            if (!Directory.Exists(PLANSAVEPATH))
            {
                Directory.CreateDirectory(PLANSAVEPATH);
            }

            var fileNames = Directory.GetFiles(PLANSAVEPATH, "*.txt");
            foreach (var fileName in fileNames)
            {
                var filePath = Path.Combine(PLANSAVEPATH, fileName);
                var fileLastModifyDate = Directory.GetLastWriteTime(filePath);
                TimeSpan diff = DateTime.Now.Date - fileLastModifyDate;
                if (fileLastModifyDate.Date == DateTime.Now || diff.Days > 10)
                {
                    Directory.Delete(filePath);
                }
            }

            var logFile = System.IO.File.Create(Path.Combine(PLANSAVEPATH, DateTime.Now.Date.ToString()));
            var logWriter = new StreamWriter(logFile);
            await logWriter.WriteLineAsync(bodyStr);
            await logWriter.DisposeAsync();
        }

        [HttpGet(Name = "GetCustOrders")]
        public async Task<IActionResult> GetCustOrders()
        {
            try
            {
                var custOrders = await _extruderOps.GetCustOrders();
                return Ok(custOrders);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetCustOrders)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("{orderNum}", Name = "GetCustOrderDetails")]
        [ResponseCache(Duration = 600)]
        public async Task<IActionResult> GetCustOrderDetails(string orderNum)
        {
            try
            {
                var custOrders = await _extruderOps.GetCOrderDetails(orderNum);
                return Ok(custOrders);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetCustOrders)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost(Name = "GetCustOrderDetailsAtOnce")]
        [ResponseCache(Duration = 600)]
        public async Task<IActionResult> GetCustOrderDetailsAtOnce([FromBody] List<string> orderNums)
        {
            try
            {
                var custOrders = await _extruderOps.GetCOrderDetails(orderNums);
                return Ok(custOrders);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetCustOrders)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        [HttpGet(Name = "GetStockReserves")]
        public async Task<IActionResult> GetStockReserves()
        {
            try
            {
                var stockReserves = await _extruderOps.GetStockReserves();
                return Ok(stockReserves);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"something went wrong in the {nameof(GetCustOrders)}");
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
                return Ok(new { prod = production, success = true });
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid post attempt in {nameof(AddProduction)}");
                return BadRequest(new { success = false, product = addProductionDTO });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMachineCaliberMapping([FromBody] MachineCaliberMapping machineCaliberMappingRequest)
        {
            var machineCaliberMapping = await _extruderOps.GetMachineCaliberMapping(machineCaliberMappingRequest.MachineId, machineCaliberMappingRequest.CaliberId);
            if (machineCaliberMapping != null)
            {
                return BadRequest();
            }

            var res = await _extruderOps.AddMachineCaliberMapping(machineCaliberMappingRequest);
            return Ok(res);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveMachineCaliberMapping([FromBody] MachineCaliberMapping machineCaliberMappingRequest)
        {
            var machineCaliberMapping = await _extruderOps.GetMachineCaliberMapping(machineCaliberMappingRequest.MachineId, machineCaliberMappingRequest.CaliberId);
            if (machineCaliberMapping == null)
            {
                return BadRequest();
            }

            var res = await _extruderOps.RemoveMachineCaliberMapping(machineCaliberMappingRequest);
            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddStockReserve([FromBody] AddStockReserveDTO addStockReserveDTO)
        {
            if (!ModelState.IsValid)
            {
                _iLogger.LogError($"Invalid post attempt in {nameof(AddStockReserve)}");
                return BadRequest(ModelState);
            }
            try
            {
                var stockReserve = _mapper.Map<StockReserve>(addStockReserveDTO);
                var oldReserves = await _unitOfWork.stockReserve.GetAll(a => a.code == stockReserve.code && a.orderNum == stockReserve.orderNum && a.usedCode == stockReserve.usedCode);
                _unitOfWork.stockReserve.DeleteRange(oldReserves);
                if (stockReserve.quantity1 > 0)
                    await _unitOfWork.stockReserve.Insert(stockReserve);
                await _unitOfWork.Save();
                return Ok(new { sReserve = stockReserve, success = true });
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid post attempt in {nameof(AddProduction)}");
                return BadRequest(new { success = false, sReserve = addStockReserveDTO });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddChosenOrders([FromBody] List<AddChosenOrderDTO> orderList)
        {
            if (!ModelState.IsValid)
            {
                _iLogger.LogError($"Invalid post attempt in {nameof(AddChosenOrders)}");
                return BadRequest(ModelState);
            }
            try
            {
                List<ChosenOrder> chosenOrders = new List<ChosenOrder>();
                foreach (var item in orderList)
                {
                    var order = _mapper.Map<ChosenOrder>(item);
                    chosenOrders.Add(order);
                }
                var allList = await _unitOfWork.chosenOrder.GetAll();
                _unitOfWork.chosenOrder.DeleteRange(allList);
                await _unitOfWork.chosenOrder.InsertRange(chosenOrders);
                await _unitOfWork.Save();
                return Ok(new { orderList = orderList, success = true });
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid post attempt in {nameof(AddProduction)}");
                return BadRequest(new { success = false, product = orderList });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChooseOrder([FromBody] Chosen chosen)
        {
            if (!ModelState.IsValid)
            {
                _iLogger.LogError($"Invalid post attempt in {nameof(ChooseOrder)}");
                return BadRequest(ModelState);
            }
            try
            {
                var machine = await _unitOfWork.machines.Get(a => a.machineName == chosen.machine);
                if (machine != null)
                {
                    machine.currentOrder = chosen.orderId;
                    _unitOfWork.machines.Update(machine);
                    await _unitOfWork.Save();
                }
                await _extruderOps.ChooseOrder(chosen);
                return Ok(new { chosen = chosen, success = true });
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid post attempt in {nameof(ChooseOrder)}");
                return BadRequest(new { success = false, chosen = chosen });
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

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid delete attempt in {nameof(DeleteProduction)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteReserves()
        {

            try
            {
                var reserves = await _unitOfWork.stockReserve.GetAll();
                if (reserves == null || reserves.Count == 0)
                {
                    _iLogger.LogError($"Invalid delete attempt in {nameof(DeleteReserves)}");
                    return BadRequest("There is nothing to delete.");
                }

                _unitOfWork.stockReserve.DeleteRange(reserves);
                await _unitOfWork.Save();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, $"Invalid delete attempt in {nameof(DeleteReserves)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
