using System;
using System.Linq;
using API_REST.Data;
using API_REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //Esse comando nos fornece auxilio, por exemplo na parte de tratamento de erros
    //Importante colocar como ControllerBase para ter mais funcionalidades para API
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        //Preparando o ambiente para manipular o banco de dados, injetando o mesmo quando a controller está sendo construída
        public ProdutosController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var produtos = _context.Produtos.ToList();
            return Ok(produtos);
            // return NotFound();  Status code 404
            // return Ok(new { nome = "Gabriel Lopes", empresa = "School of Net" }); Ocorreu como planejado, ou seja, retorna o status 200 e dados
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        //Fazendo a listagem de um produto isolado
        {
            //Vai tentar pegar algum produto do banco de dados com um determinado id, caso consiga ele retorna normalmente
            try
            {
                Produto produto = _context.Produtos.First(p => p.Id == id);
                return Ok(produto);

            }//Se ele não conseguir encontrar um produto ele irá capturar o erro e tratar ele
            catch (Exception e)
            {
                Response.StatusCode = 404; //Retorna o status Criado
                return new ObjectResult("");
                // return BadRequest(new { msg = "Id invalido!" }); Bad request é utilizadd para dizer ao cliente que algo aconteceu errado na requisição, Status 400
            }
            // return Ok("Wesley Williams " + id);
        }

        [HttpPost]
        //Recebendo um dado que vem do corpo da requisição através de um post
        public IActionResult Post([FromBody] ProdutoTemp produtoTemp)
        {
            Produto p = new Produto();
            p.Nome = produtoTemp.Nome;
            p.Preco = produtoTemp.Preco;

            _context.Produtos.Add(p);
            _context.SaveChanges();
            Response.StatusCode = 201; //Retorna o status Criado
            return new ObjectResult(""); // Funciona similar ao OK porem você precisa setar o Status Code e usar um new
            // return Ok(new { msg = "Produto criado com sucesso!" }); Responde.StatusCode = 200;
            //Criando um novo produto e informando que o mesmo foi criado, com os parametros vísiveis 
            // return Ok(new { info = "Você criou um novo produto!", produto = produtoTemp });

        }

        //Recomendado criar a classe em outro arquivo, com ela passamos os dados que queremos que sejam visualizados, nesse caso não queremos o Id
        public class ProdutoTemp
        {
            public string Nome { get; set; }
            public float Preco { get; set; }
        }


    }
}