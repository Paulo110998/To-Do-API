# TO-DO API

Esta é uma Web API REST desenvolvida com ASP.NET Core e C# para gerenciamento de tarefas, listas, anotações e membros da família.

## Funcionalidades

- Cadastro e controle de usuários
- Cadastro de listas com duplicação
- Cadastro de tarefas com recorrência (diária, semanal ou personalizada)
- Cadastro de anotações de áudio e desenho (uploads)
- Cadastro de membros da família do usuário
- Gerador de senhas automático

## Tecnologias Utilizadas

- ASP.NET Core
- C#
- Entity Framework Core
- Fluent API
- LINQ
- Identity e JWT para autenticação
- Injeção de dependências
- Princípios SOLID
- Banco de dados MySQL

## Estrutura de Diretórios

- **Authorization**: Lógica de autorização de acesso.
- **Controllers**: Controladores para cada entidade.
- **Data**: Contexto de banco de dados e DTOs.
  - **DTO**: Data Transfer Objects para cada função (Create, Read, Update).
- **Models**: Entidades.
- **Profiles**: Mapeamentos do AutoMapper.
- **Services**: Lógica de negócios.
- **Program.cs**: Configurações de inicialização.
