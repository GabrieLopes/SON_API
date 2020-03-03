using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using API_REST.Data;
using API_REST.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        [HttpPost("Registro")]
        public IActionResult Registro([FromBody] Usuario usuario)
        {
            //Verificar se as credenciais são validas
            //Verificar se o email está cadastrado no banco
            //Encriptar senha
            _context.Add(usuario);
            _context.SaveChanges();
            return Ok(new { msg = "Usuario cadastrado com sucesso." });
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] Usuario credenciais)
        {
            // buscar usuário por E-mail
            // verificar se a senha está correta
            // gerar token JWT e retornar esse token para o usuário
            try
            {
                Usuario usuario = _context.Usuarios.First(user => user.Email.Equals(credenciais.Email));
                if (usuario != null)
                {
                    // Achou usuário com cadastro válido
                    if (usuario.Senha.Equals(credenciais.Senha))
                    {
                        // Usuário achou a senha e logou!
                        
                        string ChaveDeSeguranca = "GFT_STARTER_TOKEN"; // Chave de segurança
                        var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ChaveDeSeguranca));
                        var credenciaisDeAcesso = new SigningCredentials(chaveSimetrica, SecurityAlgorithms.HmacSha256Signature);

                        var JWT = new JwtSecurityToken(
                            issuer: "Gabriel Lopes", // Quem está fornecendo o JWT para o usuário
                            expires: DateTime.Now.AddHours(1), // Expirará após tanto tempo após gerar o token
                            audience: "usuario_comum",  // Para quem é destinado esse token
                            signingCredentials: credenciaisDeAcesso
                        );
                        return Ok(new JwtSecurityTokenHandler().WriteToken(JWT));
                    }
                    else
                    {
                        Response.StatusCode = 401;  // Não autorizado
                        return new ObjectResult("");
                    }
                }
                else
                {
                    // Não achou nenhum usuário válido
                    Response.StatusCode = 401;  // Não autorizado
                    return new ObjectResult("");
                }

            }
            catch (Exception e)
            {
                Response.StatusCode = 401;  // Não autorizado
                return new ObjectResult("");
            }
        }
    }
}