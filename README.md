# Desafio Técnico - Sistema de Contas a Pagar

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-Latest-239120?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Sistema REST API para gerenciamento de contas a pagar com cálculo automático de multas e juros por atraso.

## 📋 Sobre o Projeto

Este projeto implementa um serviço REST para gerenciar contas a pagar, incluindo:
- Cadastro de contas a pagar com informações de vencimento e pagamento
- Cálculo automático de multas e juros baseado em dias de atraso
- Listagem de contas cadastradas com valores corrigidos
- Persistência de dados e regras de cálculo

## 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**, organizado em camadas:

- **DesafioTecnico.Domain**: Camada de domínio com entidades, comandos, handlers e serviços
- **DesafioTecnico.Domain.API**: Camada de apresentação (API REST)
- **DesafioTecnico.Domain.Infra**: Camada de infraestrutura (repositórios e persistência)
- **DesafioTecnico.Domain.Tests**: Testes unitários

### Padrões Utilizados

- **CQRS** (Command Query Responsibility Segregation) com MediatR
- **Repository Pattern** para acesso a dados
- **Unit of Work** para transações
- **Dependency Injection** nativo do .NET
- **Notification Pattern** com Flunt para validações

## 🚀 Tecnologias

- **.NET 10.0**
- **C# (latest)**
- **ASP.NET Core Web API**
- **MediatR 12.4.1** - Mediator pattern
- **Flunt 2.0.5** - Validações e notificações
- **Entity Framework Core 9.0.0** - ORM (In-Memory para testes)
- **Swashbuckle 7.2.0** - Documentação Swagger/OpenAPI
- **XUnit 2.9.2** - Framework de testes
- **FluentAssertions 7.0.0** - Assertions fluentes para testes
- **Coverlet** - Cobertura de código

## 📦 Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- IDE de sua preferência (Visual Studio 2022, VS Code, Rider)

## 🔧 Instalação e Execução

### 1. Clone o repositório

```bash
git clone <url-do-repositorio>
cd DesafioTecnico
```

### 2. Restaure as dependências

```bash
dotnet restore
```

### 3. Execute a aplicação

```bash
cd DesafioTecnico.Domain.API
dotnet run
```

A API estará disponível em:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:5001/swagger`

### 4. Execute os testes

```bash
dotnet test
```

### 5. Execute os testes com cobertura

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## 📚 API Endpoints

### POST /api/payableaccount
Cadastra uma nova conta a pagar e processa o pagamento.

**Request Body:**
```json
{
  "name": "Conta de Luz",
  "value": 100.00,
  "dueDate": "2026-05-01",
  "payday": "2026-05-05"
}
```

**Response (Success):**
```json
{
  "sucess": true,
  "message": "Conta a pagar registrada",
  "data": {
    "id": "guid",
    "name": "Conta de Luz",
    "value": 100.00,
    "dueDate": "2026-05-01"
  }
}
```

## 💰 Regras de Negócio

### Validações
- Todos os campos são obrigatórios
- O valor da conta deve ser maior que zero
- Data de vencimento e data de pagamento devem ser informadas

### Cálculo de Multas e Juros

O sistema aplica multas e juros progressivos baseados nos dias de atraso:

| Dias em Atraso    | Multa | Juros por Dia |
|:------------------|:-----:|:-------------:|
| Até 3 dias        |   2%  |     0,1%      |
| 4 a 5 dias        |   3%  |     0,2%      |
| 6 dias ou mais    |   5%  |     0,3%      |

**Fórmula de Cálculo:**
```
Valor com Multa = Valor Original + (Valor Original × Multa%)
Juros = Valor Original × (Dias de Atraso × Juros por Dia%)
Valor Final = Valor com Multa + Juros
```

**Exemplos:**

- **Pagamento em dia**: R$ 100,00 → R$ 100,00
- **3 dias de atraso**: R$ 100,00 → R$ 102,30
  - Multa: R$ 100 × 2% = R$ 2,00
  - Juros: R$ 100 × (3 × 0,1%) = R$ 0,30
  - Total: R$ 102,30

- **5 dias de atraso**: R$ 100,00 → R$ 104,00
  - Multa: R$ 100 × 3% = R$ 3,00
  - Juros: R$ 100 × (5 × 0,2%) = R$ 1,00
  - Total: R$ 104,00

- **6 dias de atraso**: R$ 100,00 → R$ 106,80
  - Multa: R$ 100 × 5% = R$ 5,00
  - Juros: R$ 100 × (6 × 0,3%) = R$ 1,80
  - Total: R$ 106,80

## 🧪 Testes

O projeto possui cobertura de testes unitários para:

- ✅ **Commands**: Validação de comandos
- ✅ **Handlers**: Processamento de comandos
- ✅ **Services**: Lógica de negócio (cálculo de multas e juros)
- ✅ **Entities**: Testes de entidades do domínio

### Executar Testes

```bash
# Executar todos os testes
dotnet test

# Executar com detalhes
dotnet test --logger "console;verbosity=detailed"

# Executar com cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## 📁 Estrutura do Projeto

```
DesafioTecnico/
├── DesafioTecnico.Domain/
│   ├── Commands/           # Comandos CQRS
│   ├── Entities/           # Entidades do domínio
│   ├── Handlers/           # Handlers MediatR
│   ├── Services/           # Serviços de domínio
│   └── Infra/
│       └── Storage/        # Contratos de repositórios
├── DesafioTecnico.Domain.API/
│   ├── Controllers/        # Controllers da API
│   ├── Models/             # DTOs
│   └── DependencyInjection/
├── DesafioTecnico.Domain.Infra/
│   └── (Implementações de repositórios)
└── DesafioTecnico.Domain.Tests/
    ├── Commands/           # Testes de comandos
    ├── Handlers/           # Testes de handlers
    ├── Services/           # Testes de serviços
    ├── Entities/           # Testes de entidades
    └── FakeRepositories/   # Mocks para testes
```

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 👤 Autor

Desenvolvido como parte de um desafio técnico para vaga de Analista Desenvolvedor .NET | .NET Core - Pleno.

---

⭐ Se este projeto foi útil para você, considere dar uma estrela!
