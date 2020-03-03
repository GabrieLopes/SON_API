using API_REST.Data;
using API_REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_REST.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        //Preparando o ambiente para manipular o banco de dados, injetando o mesmo quando a controller está sendo construída
        public UsuariosController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // api/v1/usuarios/registro
        [HttpPost("registro")]
        public IActionResult Registro([FromBody] Usuario usuario)
        {
            //Verificar se as credenciais são validas
            //Verificar se o email está cadastrado no banco
            //Encriptar senha
            _context.Add(usuario);
            _context.SaveChanges();
            return Ok(new { msg = "Usuario cadastrado com sucesso." });
        }
    }
}