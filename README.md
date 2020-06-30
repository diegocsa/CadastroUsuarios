# CadastroUsuarios
Cadastro de Usuários com .net core , Testes, EFCore e JWT e SQL Server

Para executar, basta satisfazer as dependencias do NuGet, alterar a connection strign no appsettings.json ( o default é "Server=(localdb)\mssqllocaldb;Database=CadastroUsuarios;Trusted_Connection=True;")

AO executar, as migrations ja vão criar a base e as tabelas

Os POST 'Usuario' e 'Usuario/Login' não precisam de autenticação
