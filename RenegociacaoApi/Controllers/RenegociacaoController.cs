using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RenegociacaoController : ControllerBase
{
    private readonly RenegociacaoService _service = new();

    [HttpPost]
    public IActionResult Renegociar([FromBody] RenegociacaoRequest request)
    {
        try
        {
            var result = _service.Renegociar(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}