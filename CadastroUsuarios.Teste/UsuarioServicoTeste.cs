using CadastroUsuarios.Dominio.Entidades;
using CadastroUsuarios.Dominio.Exceptions;
using CadastroUsuarios.Dominio.Interfaces.Repository;
using CadastroUsuarios.Dominio.Models;
using CadastroUsuarios.Infra.CrossCutting;
using CadastroUsuarios.Servico;
using Moq;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using Xunit;

namespace CadastroUsuarios.Teste
{
    public class UsuarioServicoTeste
    {

        /*
         A nomenclatura dos métodos de teste, segue a estrutura:
        MetodoSobTestes_Parametros_Assert
         */
        
        private Mock<IUsuarioRepository> _repository;
        private UsuarioService _serviceSobTeste;

        private AlterarUsuarioModel alterarUsuarioValido;
        private NovoUsuarioModel novoUsuarioValido;
        private TrocaSenhaModel trocaSenhaValido;

        public UsuarioServicoTeste()
        {
            _repository = new Mock<IUsuarioRepository>();
            _serviceSobTeste = new UsuarioService(_repository.Object);

            novoUsuarioValido = new NovoUsuarioModel()
            {
                Nome = "José",
                EMail = "jose@email.com",
                Senha = "x123",
                ConfirmacaoSenha = "x123"
            };

            alterarUsuarioValido = new AlterarUsuarioModel()
            {
                Id = new Guid("05d82291-cea4-4362-b550-07b989a08cbb"),
                Nome = "Pedro"
            };

            trocaSenhaValido = new TrocaSenhaModel()
            {
                Id = new Guid("05d82291-cea4-4362-b550-07b989a08cbb"),
                SenhaAtual = "x123",
                NovaSenha = "x1234",
                ConfirmacaoNovaSenha = "x1234",
            };

            //Registrar TinyMapper
            TinyMapper.Bind<Usuario, UsuarioModel>();
            TinyMapper.Bind<List<Usuario>, List<UsuarioModel>>();
            TinyMapper.Bind<Usuario, NovoUsuarioModel>();
            TinyMapper.Bind<Usuario, AlterarUsuarioModel>();
            TinyMapper.Bind<NovoUsuarioModel, Usuario>();
            TinyMapper.Bind<AlterarUsuarioModel, Usuario>();

        }

        #region Alterar
        [Fact]
        public void Alterar_DadosValidos_ExecutaAlterarRepository()
        {
            _repository.Setup(x => x.Recuperar(It.IsAny<Guid>())).Returns(new Usuario());
            _serviceSobTeste.Alterar(alterarUsuarioValido);
            _repository.Verify(x => x.Alterar(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public void Alterar_IdUusarioNaoExiste_Exception()
        {
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.Alterar(alterarUsuarioValido));
            _repository.Verify(x => x.Alterar(It.IsAny<Usuario>()), Times.Never);
            Assert.Equal("Nenhum usuário encontrato para o id informado.", ex.Message);
        }

        [Fact]
        public void Alterar_ObjetoUsuarioAlteracaoNull_Exception()
        {
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.Alterar(null));
            _repository.Verify(x => x.Alterar(It.IsAny<Usuario>()), Times.Never);
            Assert.Equal("Nenhum usuário informado.", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Alterar_NomeAlteracaoInvalido_Exception(string nome)
        {
            alterarUsuarioValido.Nome = nome;
            _repository.Setup(x => x.Recuperar(It.IsAny<Guid>())).Returns(new Usuario());

            _repository.Verify(x => x.Alterar(It.IsAny<Usuario>()), Times.Never);
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.Alterar(alterarUsuarioValido));
            Assert.Equal("'Nome' não foi informado.", ex.Message);
        }
        #endregion

        #region Excluir
        [Fact]
        public void Excluir_GuidValido_ExecutaExcluirRepository()
        {
            _repository.Setup(x => x.Recuperar(It.IsAny<Guid>())).Returns(new Usuario());
            _serviceSobTeste.Excluir(Guid.NewGuid());
            _repository.Verify(x => x.Excluir(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void Excluir_GuidNenhumUsuario_Exception()
        {
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.Excluir(Guid.NewGuid()));
            _repository.Verify(x => x.Excluir(It.IsAny<Guid>()), Times.Never);
            Assert.Equal("Nenhum usuário encontrado para o id informado.", ex.Message);
        }
        #endregion

        #region Inserir
        [Fact]
        public void Inserir_DadosValidos_ExecutaInserirRepositoryERetornaUsuarioComId()
        {
            var usuario = _serviceSobTeste.Inserir(novoUsuarioValido);
            
            Assert.True(usuario.Id != Guid.Empty);
            _repository.Verify(x => x.Inserir(It.IsAny<Usuario>()), Times.Once);
            _repository.Verify(x => x.Recuperar(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Inserir_ObjetoNull_Exception()
        {
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.Inserir(null));
            _repository.Verify(x => x.Inserir(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<string>()), Times.Never);
            Assert.Equal("Dados do usuario não foram informados.", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Inserir_NomeNaoInformado_Exception(string nome)
        {
            novoUsuarioValido.Nome = nome;
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.Inserir(novoUsuarioValido));
            _repository.Verify(x => x.Inserir(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<string>()), Times.Never);
            Assert.Equal("'Nome' não foi informado.", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Inserir_EmailNaoInformado_Exception(string email)
        {
            novoUsuarioValido.EMail = email;
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.Inserir(novoUsuarioValido));
            _repository.Verify(x => x.Inserir(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<string>()), Times.Never);
            Assert.Equal("'EMail' não foi informado.", ex.Message);
        }

        [Fact]
        public void Inserir_EmailInvalido_Exception()
        {
            novoUsuarioValido.EMail = "djfhapfuaposi";
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.Inserir(novoUsuarioValido));
            _repository.Verify(x => x.Inserir(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<string>()), Times.Never);
            Assert.Equal("'EMail' inválido.", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Inserir_SenhaNaoInformada_Exception(string senha)
        {
            novoUsuarioValido.Senha = senha;
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.Inserir(novoUsuarioValido));
            _repository.Verify(x => x.Inserir(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<string>()), Times.Never);
            Assert.Equal("'Senha' não foi informada.", ex.Message);
        }

        [Fact]
        public void Inserir_EmailJaUtilizado_Exception()
        {
            novoUsuarioValido.EMail = "teste@teste.com.br";
            _repository.Setup(x => x.Recuperar("teste@teste.com.br")).Returns(new Usuario());
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.Inserir(novoUsuarioValido));
            _repository.Verify(x => x.Inserir(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar("teste@teste.com.br"), Times.Once);
            Assert.Equal("'EMail' já cadastrado.", ex.Message);
        }

        [Fact]
        public void Inserir_SenhaDifereDaConfirmacao_Exception()
        {
            novoUsuarioValido.ConfirmacaoSenha = "XX";
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.Inserir(novoUsuarioValido));
            _repository.Verify(x => x.Inserir(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<string>()), Times.Never);
            Assert.Equal("'Senha' e 'ConfirmacaoSenha' não foram informados ou não são iguais.", ex.Message);
        }
        #endregion

        #region TrocarSenha
        [Fact]
        public void TrocarSenha_DadosValidos_ExecutaAlteracao()
        {
            _repository.Setup(x => x.Recuperar(trocaSenhaValido.Id)).Returns(new Usuario() { Id = trocaSenhaValido.Id, Senha = trocaSenhaValido.SenhaAtual.TratarComoSenha() });

            _serviceSobTeste.TrocarSenha(trocaSenhaValido);

            var usuario = new Usuario()
            {
                Id = trocaSenhaValido.Id,
                Senha = trocaSenhaValido.NovaSenha.TratarComoSenha()
            };

            _repository.Verify(x => x.Recuperar(trocaSenhaValido.Id), Times.Once);
            _repository.Verify(x => x.Alterar(usuario), Times.Once);
        }

        [Fact]
        public void TrocarSenha_ObjetoNull_Exception()
        {
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.TrocarSenha(null));
            _repository.Verify(x => x.Alterar(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<Guid>()), Times.Never);
            Assert.Equal("Dados de troca de senha não foram informados.", ex.Message);
        }

        [Fact]
        public void TrocarSenha_IdInvalido_Exception()
        {
            trocaSenhaValido.Id = Guid.Empty;
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.TrocarSenha(trocaSenhaValido));
            _repository.Verify(x => x.Alterar(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<Guid>()), Times.Never);
            Assert.Equal("'Id' inválido.", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TrocarSenha_SenhaAtualNaoInformada_Exception(string senhaAtual)
        {
            trocaSenhaValido.SenhaAtual = senhaAtual;
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.TrocarSenha(trocaSenhaValido));
            _repository.Verify(x => x.Alterar(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<Guid>()), Times.Never);
            Assert.Equal("'SenhaAtual' não foi informada.", ex.Message);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData("aaa", null)]
        [InlineData("aaa", "")]
        [InlineData("aaa", "bbb")]
        [InlineData(null, "bbb")]
        [InlineData("", "bbb")]
        public void TrocarSenha_SenhaEConfirmacaoNaoIformadasOuDiferentes_Exception(string nova, string confirmacao)
        {
            trocaSenhaValido.NovaSenha = nova;
            trocaSenhaValido.ConfirmacaoNovaSenha = confirmacao;
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.TrocarSenha(trocaSenhaValido));
            _repository.Verify(x => x.Alterar(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<Guid>()), Times.Never);
            Assert.Equal("'NovaSenha' e 'ConfirmacaoNovaSenha' não foram informados ou não são iguais.", ex.Message);
        }

        [Fact]
        public void TrocarSenha_IdNaoPertenceANinguem_Exception()
        {
            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.TrocarSenha(trocaSenhaValido));
            _repository.Verify(x => x.Alterar(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<Guid>()), Times.Once);
            Assert.Equal("Nenhum usuário encontrado para o id informado.", ex.Message);
        }

        [Fact]
        public void TrocarSenha_SenhaAtualNaoConfere_Exception()
        {
            _repository.Setup(x => x.Recuperar(trocaSenhaValido.Id)).Returns(new Usuario() { Senha = "XXXXXXX".TratarComoSenha()});

            var ex = Assert.Throws<RegraVioladaException>(() => _serviceSobTeste.TrocarSenha(trocaSenhaValido));
            _repository.Verify(x => x.Alterar(It.IsAny<Usuario>()), Times.Never);
            _repository.Verify(x => x.Recuperar(It.IsAny<Guid>()), Times.Once);
            Assert.Equal("A senha atual não confere.", ex.Message);
        }
        #endregion


        #region Listar
        [Fact]
        public void Listar_NenhumParametro_ExecutaListarRepository()
        {
            _repository.Setup(x => x.Listar()).Returns(new List<Usuario>());
            _serviceSobTeste.Listar();
            _repository.Verify(x => x.Listar(), Times.Once);
        }
        #endregion

        #region Recuperar
        [Fact]
        public void Recuperar_PorGuid_ExecutaRecuperarComRepositoryComMesmoGuid()
        {
            var guid = Guid.NewGuid();

            _repository.Setup(x => x.Recuperar(It.IsAny<Guid>())).Returns(new Usuario());
            _serviceSobTeste.Recuperar(guid);
            _repository.Verify(x => x.Recuperar(guid), Times.Once);
        }

        [Fact]
        public void Recuperar_PorEmail_ExecutaRecuperarComRepositoryComMesmoEmail()
        {
            var email = "teste@teste.com";

            _repository.Setup(x => x.Recuperar(It.IsAny<string>())).Returns(new Usuario());
            _serviceSobTeste.Recuperar(email);
            _repository.Verify(x => x.Recuperar(email), Times.Once);
        }
        #endregion

        #region UsuarioValido
        [Fact]
        public void UsuarioValido_LoginSenhaValidos_True()
        {
            string login = "teste@teste.com.br";
            string senha = "abc";
            
            _repository.Setup(x => x.Recuperar(login)).Returns(new Usuario() { Senha = "abc".TratarComoSenha() });

            Assert.True(_serviceSobTeste.UsuarioValido(login, senha));
            _repository.Verify(x => x.Recuperar(login), Times.Once);
        }

        [Fact]
        public void UsuarioValido_LoginNaoExisteInvalidos_False()
        {
            string login = "teste@teste.com.br";
            string senha = "abc";

            Assert.False(_serviceSobTeste.UsuarioValido(login, senha));
            _repository.Verify(x => x.Recuperar(login), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("XXXX")]
        public void UsuarioValido_SenhaNaoConfere_False(string senha)
        {
            string login = "teste@teste.com.br";

            _repository.Setup(x => x.Recuperar(login)).Returns(new Usuario() { Senha = "abc".TratarComoSenha() });

            Assert.False(_serviceSobTeste.UsuarioValido(login, senha));
            _repository.Verify(x => x.Recuperar(login), Times.Once);
        }
        #endregion
    }
}
