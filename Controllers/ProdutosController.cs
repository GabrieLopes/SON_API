using Microsoft.AspNetCore.Mvc;

namespace API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // return NotFound();  Status code 404
            return Ok(new {nome = "Gabriel Lopes", empresa = "School of Net" }); //Ocorreu como planejado, ou seja retorna o status 200 e dados
        }    

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok("Wesley Williams " + id);
        }
    }
}