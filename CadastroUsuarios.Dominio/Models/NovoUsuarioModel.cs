namespace CadastroUsuarios.Dominio.Models
{
    public class NovoUsuarioModel
    {
        public string Nome { get; set; }
        public string EMail { get; set; }
        public string Senha { get; set; }
        public string ConfirmacaoSenha { get; set; }
    }
}
