using CadastroUsuarios.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadastroUsuarios.Dominio.Interfaces.Repository
{
    public interface IUsuarioRepository
    {
        void Inserir(Usuario usuario);
        void Alterar(Usuario usuario);
        void Excluir(Guid id);
        IEnumerable<Usuario> Listar();
        Usuario Recuperar(Guid id);
        Usuario Recuperar(string email);

    }
}
