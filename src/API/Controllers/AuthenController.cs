using EXAM_SYSTEM_API.API.Shared;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using Microsoft.AspNetCore.Mvc;

namespace EXAM_SYSTEM_API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthenService _service;
        private readonly ControllerHelper _helper;

        public AuthenController(IAuthenService service, ControllerHelper helper)
        {
            _service = service;
            _helper = helper;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequestDTO)
        {
            return await _helper.HandleRequest(() => _service.Login(loginRequestDTO));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequest registerationRequestDTO)
        {
            return await _helper.HandleRequest(() => _service.Register(registerationRequestDTO));
        }
    }
}
