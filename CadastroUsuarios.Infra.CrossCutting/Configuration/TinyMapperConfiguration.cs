using CadastroUsuarios.Dominio.Entidades;
using CadastroUsuarios.Dominio.Models;
using Microsoft.Extensions.DependencyInjection;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadastroUsuarios.Infra.CrossCutting.Configuration
{
    public static class TinyMapperConfiguration
    {
        public static void AddTinyMapperConfiguration(this IServiceCollection services)
        {
            TinyMapper.Bind<Usuario, UsuarioModel>();
            TinyMapper.Bind<List<Usuario>, List<UsuarioModel>>();
            TinyMapper.Bind<Usuario, NovoUsuarioModel>();
            TinyMapper.Bind<Usuario, AlterarUsuarioModel>();
            TinyMapper.Bind<NovoUsuarioModel, Usuario>();
            TinyMapper.Bind<AlterarUsuarioModel, Usuario>();
        }
    }
}
