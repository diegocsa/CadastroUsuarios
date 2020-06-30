using System;
using System.Text.Json.Serialization;

namespace CadastroUsuarios.Dominio.Models
{
    public class TrocaSenhaModel
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
        public string ConfirmacaoNovaSenha { get; set; }
    }
}
