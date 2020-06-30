using System;
using System.Collections.Generic;
using System.Text;

namespace CadastroUsuarios.Dominio.Entidades
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string EMail { get; set; }
        public string Senha { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var x = (Usuario)obj;
            return Id == x.Id
                && Nome == x.Nome
                && EMail == x.EMail
                && Senha == x.Senha;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nome, EMail, Senha);
        }
    }
}
