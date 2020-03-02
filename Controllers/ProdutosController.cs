using API_REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //Importante colocar como ControllerBase para ter mais funcionalidades para API
    public class ProdutosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // return NotFound();  Status code 404
            return Ok(new { nome = "Gabriel Lopes", empresa = "School of Net" }); //Ocorreu como planejado, ou seja retorna o status 200 e dados
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok("Wesley Williams " + id);
        }

        [HttpPost]
        //Recebendo um dado que vem do corpo da requisição através de um post
        public IActionResult Post([FromBody] ProdutoTemp produtoTemp)
        {
            // return Ok(produtoTemp);
            //Criando um novo produto e informando que o mesmo foi criado, com os parametros vísiveis 
            return Ok(new { info = "Você criou um novo produto!", produto = produtoTemp });
        }

        //Recomendado criar a classe em outro arquivo, com ela passamos os dados que queremos que sejam visualizados, nesse caso não queremos o Id
        public class ProdutoTemp
        {
            public string Nome { get; set; }
            public float Preco { get; set; }
        }


    }
}