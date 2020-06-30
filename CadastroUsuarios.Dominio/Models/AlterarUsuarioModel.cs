using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CadastroUsuarios.Dominio.Models
{
    public class AlterarUsuarioModel
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Nome { get; set; }
    }
}
