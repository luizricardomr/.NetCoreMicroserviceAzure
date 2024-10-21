using Mango.ServiceBus;
using Mango.Services.AuthApi.Models.DTO;
using Mango.Services.AuthApi.RabbitMQSender;
using Mango.Services.AuthApi.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _service;
        private readonly IRabbitMQAuthMessageSender _rabbitMessage;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        protected ResponseDTO _response;

        public AuthController(IAuthService service, IMessageBus messageBus, IConfiguration configuration, IRabbitMQAuthMessageSender rabbitMessage)
        {
            _service = service;
            _response = new();
            _messageBus = messageBus;
            _configuration = configuration;
            _rabbitMessage = rabbitMessage;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterarionRequestDTO model)
        {
            var errorMessage = await _service.Register(model);
            
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
           await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));
           _rabbitMessage.SendMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));

			return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            var loginResponse = await _service.Login(dto);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or Password is invalid";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(RegisterarionRequestDTO dto)
        {
            var assignRoleSuccessful = await _service.AssignRole(dto.Email, dto.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";
                return BadRequest(_response);
            }

            return Ok(_response);
        }
    }
}
