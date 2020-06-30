using CadastroUsuarios.Dominio.Entidades;
using CadastroUsuarios.Dominio.Exceptions;
using CadastroUsuarios.Dominio.Interfaces.Repository;
using CadastroUsuarios.Dominio.Models;
using CadastroUsuarios.Infra.CrossCutting;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CadastroUsuarios.Servico
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }


        public void Alterar(AlterarUsuarioModel usuario)
        {
            if (usuario == null)
                throw new RegraVioladaException("Nenhum usuário informado.");

            var atual = _repository.Recuperar(usuario.Id);

            if (atual == null)
                throw new RegraVioladaException("Nenhum usuário encontrato para o id informado.");

            if (string.IsNullOrEmpty(usuario.Nome))
                throw new RegraVioladaException("'Nome' não foi informado.");

            var entidade = TinyMapper.Map<Usuario>(usuario);

            _repository.Alterar(entidade);
        }

        public void Excluir(Guid id)
        {
            var atual = _repository.Recuperar(id);

            if (atual == null)
                throw new RegraVioladaException("Nenhum usuário encontrado para o id informado.");

            _repository.Excluir(id);
        }

        public UsuarioModel Inserir(NovoUsuarioModel usuario)
        {
            if (usuario == null)
                throw new RegraVioladaException("Dados do usuario não foram informados.");

            if (string.IsNullOrEmpty(usuario.Nome))
                throw new RegraVioladaException("'Nome' não foi informado.");

            if (string.IsNullOrEmpty(usuario.EMail))
                throw new RegraVioladaException("'EMail' não foi informado.");

            if (!usuario.EMail.EmailValido())
                throw new RegraVioladaException("'EMail' inválido.");

            if (string.IsNullOrEmpty(usuario.Senha))
                throw new RegraVioladaException("'Senha' não foi informada.");

            if (string.IsNullOrEmpty(usuario.Senha) || string.IsNullOrEmpty(usuario.ConfirmacaoSenha) || (usuario.Senha != usuario.ConfirmacaoSenha))
                throw new RegraVioladaException("'Senha' e 'ConfirmacaoSenha' não foram informados ou não são iguais.");

            if (_repository.Recuperar(usuario.EMail) != null)
                throw new RegraVioladaException("'EMail' já cadastrado.");

            var entidade = TinyMapper.Map<Usuario>(usuario);
            entidade.Senha = entidade.Senha.TratarComoSenha();
            entidade.Id = Guid.NewGuid();
            _repository.Inserir(entidade);
            return TinyMapper.Map<UsuarioModel>(entidade);
        }

        public IEnumerable<Usuario> Listar() => _repository.Listar().ToList();
        public Usuario Recuperar(Guid id) => _repository.Recuperar(id);
        public Usuario Recuperar(string email) => _repository.Recuperar(email);

        public void TrocarSenha(TrocaSenhaModel trocaSenha)
        {
            if (trocaSenha == null)
                throw new RegraVioladaException("Dados de troca de senha não foram informados.");

            if (trocaSenha.Id == Guid.Empty)
                throw new RegraVioladaException("'Id' inválido.");

            if (string.IsNullOrEmpty(trocaSenha.SenhaAtual))
                throw new RegraVioladaException("'SenhaAtual' não foi informada.");

            if (string.IsNullOrEmpty(trocaSenha.NovaSenha) || string.IsNullOrEmpty(trocaSenha.ConfirmacaoNovaSenha) || (trocaSenha.NovaSenha != trocaSenha.ConfirmacaoNovaSenha))
                throw new RegraVioladaException("'NovaSenha' e 'ConfirmacaoNovaSenha' não foram informados ou não são iguais.");

            var usuario = _repository.Recuperar(trocaSenha.Id);

            if (usuario == null)
                throw new RegraVioladaException("Nenhum usuário encontrado para o id informado.");

            if (usuario.Senha != trocaSenha.SenhaAtual.TratarComoSenha())
                throw new RegraVioladaException("A senha atual não confere.");

            usuario.Senha = trocaSenha.NovaSenha.TratarComoSenha();

            _repository.Alterar(usuario);
        }

        public bool UsuarioValido(string login, string senha)
        {
            var usuario = _repository.Recuperar(login);

            if (usuario != null && !string.IsNullOrEmpty(senha))
                return usuario.Senha == senha.TratarComoSenha();

            return false;
        }
    }
}
