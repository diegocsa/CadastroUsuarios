using CadastroUsuarios.Dominio.Entidades;
using CadastroUsuarios.Dominio.Models;
using System;
using System.Collections.Generic;

namespace CadastroUsuarios.Dominio.Interfaces.Repository
{
    public interface IUsuarioService
    {
        UsuarioModel Inserir(NovoUsuarioModel usuario);
        void TrocarSenha(TrocaSenhaModel trocaSenha);
        void Alterar(AlterarUsuarioModel usuario);
        void Excluir(Guid id);
        IEnumerable<Usuario> Listar();
        Usuario Recuperar(Guid id);
        Usuario Recuperar(string email);
        bool UsuarioValido(string login, string senha);
    }
}
