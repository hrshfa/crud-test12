using Mc2.CrudTest.Presentation.Data.Repositories;
using Mc2.CrudTest.Presentation.Server.Services;
using Mc2.CrudTest.Shared.Helpers;
using Mc2.CrudTest.Shared.Models;
using Mc2.CrudTest.Shared.Models.Args;
using Mc2.CrudTest.Shared.Models.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mc2.CrudTest.Presentation.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomersRepository _customerRepository;
        private readonly RabbitMQProducerService _rabbitMQProducerService;

        public CustomersController(ILogger<CustomersController> logger, ICustomersRepository customerRepository, RabbitMQProducerService rabbitMQProducerService)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _rabbitMQProducerService = rabbitMQProducerService;
        }
        // GET: <CustomersController>
        [HttpPost("GetAll")]
        public async Task<IActionResult> Get([FromBody] RadzenLoadDataArgsModel args)
        {
            ResponseDTO<RadzenDataGridDataModel<CustomerDto>> response = new();
            try
            {
                var models = await _customerRepository.Get(args);
                response.IsSuccessful = true;
                response.Data = models;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ErrorHandlingHelper.ParseError(_logger, "Get", ex));
                return Ok(response);
            }
        }
        // GET <CustomersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            ResponseDTO<CustomerDto> response = new();
            try
            {
                var model = await _customerRepository.GetByID(id ?? throw new Exception("id must have value."));
                response.IsSuccessful = true;
                response.Data = model;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ErrorHandlingHelper.ParseError(_logger, "GetByID", ex));
                return Ok(response);
            }
        }
        // POST <CustomersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerDto value)
        {
            ResponseDTO<CustomerDto> response = new();
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(a => a.Type == "sid")?.Value ?? throw new Exception("UserId must have value.");
                var model = await _customerRepository.Insert(value);
                model.UserId = userId;
                await _rabbitMQProducerService.AddToLogQueueAsync(model);
                response.IsSuccessful = true;
                response.Data = value;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ErrorHandlingHelper.ParseError(_logger, "Post", ex));
                return Ok(response);
            }
        }
        // PUT <CustomersController>
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] CustomerDto value)
        {
            VoidResponseDTO response = new();
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(a => a.Type == "sid")?.Value ?? throw new Exception("UserId must have value.");
                var model = await _customerRepository.Update(value);
                model.UserId = userId;
                await _rabbitMQProducerService.AddToLogQueueAsync(model);
                response.IsSuccessful = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ErrorHandlingHelper.ParseError(_logger, "Put", ex));
                return Ok(response);
            }
        }
        // DELETE <CustomersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            VoidResponseDTO response = new();
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(a => a.Type == "sid")?.Value ?? throw new Exception("UserId must have value.");
                var model = await _customerRepository.Delete(id ?? throw new Exception("Id must have value."));
                model.UserId = userId;
                await _rabbitMQProducerService.AddToLogQueueAsync(model);
                response.IsSuccessful = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ErrorHandlingHelper.ParseError(_logger, "Delete", ex));
                return Ok(response);
            }
        }
    }
}
