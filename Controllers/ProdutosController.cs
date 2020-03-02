using System;
using System.Collections.Generic;
using System.Linq;
using API_REST.Data;
using API_REST.HATEOAS;
using API_REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_REST.Controllers
{
    [Route("api/v1/[controller]")] // Versão Legado - Versão sem suporte!!
    [ApiController] //Esse comando nos fornece auxilio, por exemplo na parte de tratamento de erros
    //Importante colocar como ControllerBase para ter mais funcionalidades para API
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private HATEOAS.HATEOAS HATEOAS;

        //Preparando o ambiente para manipular o banco de dados, injetando o mesmo quando a controller está sendo construída
        public ProdutosController(ApplicationDbContext context)
        {
            this._context = context;
            HATEOAS = new HATEOAS.HATEOAS("localhost:5001/api/v1/Produtos"); //HATEOAS que aponta para essa url
            HATEOAS.AddAction("GET_INFO", "GET");
            HATEOAS.AddAction("DELETE_PRODUCT", "DELETE");
            HATEOAS.AddAction("EDIT_PRODUCT", "PATCH");
        }

        [HttpGet]
        public IActionResult Get()
        {
            var produtos = _context.Produtos.ToList();
            List<ProdutoContainer> produtosHATEOAS = new List<ProdutoContainer>();
            foreach(var prod in produtos)
            {
                ProdutoContainer produtoHATEOAS = new ProdutoContainer();
                produtoHATEOAS.produto = prod;
                produtoHATEOAS.links = HATEOAS.GetActions(prod.Id.ToString());
                produtosHATEOAS.Add(produtoHATEOAS);
            }
            return Ok(produtosHATEOAS);
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
                ProdutoContainer produtoHATEOAS = new ProdutoContainer();
                produtoHATEOAS.produto = produto;
                produtoHATEOAS.links = HATEOAS.GetActions(produto.Id.ToString());
                return Ok(produtoHATEOAS);

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
            /* Validação */
            if (produtoTemp.Preco <= 0)
            {
                Response.StatusCode = 400;
                return new ObjectResult(new { msg = "O preço do produto não pode ser menor ou igual a 0." });
            }
            if (produtoTemp.Nome.Length <= 1)
            {
                Response.StatusCode = 400;
                return new ObjectResult(new { msg = "O nome do produto precisa ter mais do que 1 caracter." });
            }

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

        [HttpDelete("{id}")] //Especificando que irá trabalhar com um Id
        public IActionResult Delete(int id)
        {
            try
            {
                Produto produto = _context.Produtos.First(p => p.Id == id);
                _context.Produtos.Remove(produto);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                Response.StatusCode = 404;
                return new ObjectResult("");
            }
        }

        [HttpPatch()]
        public IActionResult Patch([FromBody] Produto produto)
        {
            if (produto.Id > 0)
            {
                try
                {
                    var p = _context.Produtos.First(produtoTemp => produtoTemp.Id == produto.Id);
                    if (p != null)
                    {
                        //Editar

                        //"IF ELSE reduzido"  
                        /*Condição ? faz algo : faz outra coisa, ou seja, se o dado nome for diferente de nulo que vem da minha requisição eu altero o nome do produto pelo nome
                        que vem na minha requisição, senão o nome que veio na requisição é nulo ele mantem o nome do produto*/
                        p.Nome = produto.Nome != null ? produto.Nome : p.Nome;
                        p.Preco = produto.Preco != 0 ? produto.Preco : p.Preco;

                        _context.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        return new ObjectResult(new { msg = "Produto não encontrado." });
                    }
                }
                catch
                {
                    Response.StatusCode = 400;
                    return new ObjectResult(new { msg = "Produto não encontrado." });
                }

            }
            else
            {
                Response.StatusCode = 400;
                return new ObjectResult(new { msg = "Id do produto é invalido." });
            }
        }

        //Recomendado criar a classe em outro arquivo, com ela passamos os dados que queremos que sejam visualizados, nesse caso não queremos o Id
        public class ProdutoTemp
        {
            public string Nome { get; set; }
            public float Preco { get; set; }
        }

        public class ProdutoContainer
        {
            public Produto produto { get; set; }
            public Link[] links { get; set; }
        }


    }
}