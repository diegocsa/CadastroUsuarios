using CadastroUsuarios.Dominio.Exceptions;
using CadastroUsuarios.Dominio.Interfaces.Repository;
using CadastroUsuarios.Dominio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Nelibur.ObjectMapper;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CadastroUsuarios.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly IConfiguration _configuration;

        public UsuarioController(IUsuarioService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var lista = _service.Listar();
            
            if (lista == null || !lista.Any())
                return NoContent();
            
            return Ok(TinyMapper.Map<List<UsuarioModel>>(lista));
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public IActionResult Get(Guid id)
        {
            var usuario = _service.Recuperar(id);
            
            if (usuario == null)
                return NoContent();
            
            return Ok(TinyMapper.Map<UsuarioModel>(usuario));
        }

        [HttpPost]
        public IActionResult Post([FromBody] NovoUsuarioModel usuario)
        {
            try
            {
                return Created(string.Empty, _service.Inserir(usuario));
            }
            catch (RegraVioladaException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Um erro inesperado foi encontrato ao processar a requisição");
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            try
            {
                if(_service.UsuarioValido(login.Login, login.Senha))
                {
                    return Ok(new
                    {
                        token = Infra.CrossCutting.JWT.GerarToken(_configuration["TokenJWT:Issuer"], _configuration["TokenJWT:Audience"], DateTime.Now.AddMinutes(60), _configuration["TokenJWT:Chave"])
                    });
                }
                
                return BadRequest("Usuario/Senha inválido");
            }
            catch (RegraVioladaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Um erro inesperado foi encontrato ao processar a requisição");
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public IActionResult Put(Guid id, [FromBody] AlterarUsuarioModel usuario)
        {
            try
            {
                usuario.Id = id;
                _service.Alterar(usuario);
                
                return NoContent();
            }
            catch (RegraVioladaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Um erro inesperado foi encontrato ao processar a requisição");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _service.Excluir(id);
             
                return NoContent();
            }
            catch (RegraVioladaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Um erro inesperado foi encontrato ao processar a requisição");
            }
        }

        [HttpPut("{id:guid}/trocarSenha")]
        [Authorize]
        public IActionResult TrocarSenha(Guid id, [FromBody] TrocaSenhaModel usuario)
        {
            try
            {
                usuario.Id = id;
                _service.TrocarSenha(usuario);
             
                return NoContent();
            }
            catch (RegraVioladaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Um erro inesperado foi encontrato ao processar a requisição");
            }
        }
    }
}
