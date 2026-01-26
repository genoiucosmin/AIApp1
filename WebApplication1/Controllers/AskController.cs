using Microsoft.AspNetCore.Mvc;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("ask")]
    public class AskController : ControllerBase
    {
        private readonly AiService _ai;

        public AskController(AiService ai)
        {
            _ai = ai;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] AskRequest request)
        {
            var answer = await _ai.Ask(request.Question);
            return Ok(new { answer });
        }
    }

    public record AskRequest(string Question);

}
