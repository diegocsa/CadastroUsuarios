using System;
using System.Collections.Generic;
using System.Text;

namespace CadastroUsuarios.Dominio.Models
{
    public class UsuarioModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string EMail { get; set; }
    }
}
