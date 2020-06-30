using CadastroUsuarios.Dominio.Entidades;
using CadastroUsuarios.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CadastroUsuarios.Infra.Data
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private DbSet<Usuario> _dbSet;
        private PrincipalContext _context;

        public UsuarioRepository(PrincipalContext context)
        {
            _context = context;
            _dbSet = _context.Set<Usuario>();
        }
        public void Alterar(Usuario usuario)
        {
            _dbSet.Attach(usuario);
            _context.Entry(usuario).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Excluir(Guid id)
        {
            var usuario = _dbSet.Find(id);
            _dbSet.Attach(usuario);
            _dbSet.Remove(usuario);
            _context.SaveChanges();
        }

        public void Inserir(Usuario usuario)
        {
            _dbSet.Add(usuario);
            _context.SaveChanges();
        }

        public IEnumerable<Usuario> Listar()
        {
            return _dbSet;
        }

        public Usuario Recuperar(Guid id)
        {
            return _dbSet.SingleOrDefault(x => x.Id == id);
        }

        public Usuario Recuperar(string email)
        {
            return _dbSet.SingleOrDefault(x => x.EMail == email);

        }
    }
}
