# Changelog

## [2.0.0] - 2026-05-05 - Upgrade para .NET 10

### 🚀 Atualizações Principais

#### Framework e Runtime
- ✅ Migrado de .NET Core 3.1/5.0 para **.NET 10.0**
- ✅ Habilitado **Nullable Reference Types** em todos os projetos
- ✅ Atualizado para **C# Latest**

#### Dependências Atualizadas

| Pacote | Versão Anterior | Nova Versão |
|--------|----------------|-------------|
| MediatR | 9.0.0 | **12.4.1** |
| Flunt | 1.0.5 | **2.0.5** |
| Entity Framework Core | 5.0.1 | **9.0.0** |
| Swashbuckle.AspNetCore | 5.6.3 | **7.2.0** |
| XUnit | 2.4.1 | **2.9.2** |
| Microsoft.NET.Test.Sdk | 16.7.1 | **17.12.0** |
| xunit.runner.visualstudio | 2.4.3 | **2.8.2** |
| coverlet.collector | 1.3.0 | **6.0.2** |

#### Novas Dependências
- ✅ **FluentAssertions 7.0.0** - Assertions mais expressivas e legíveis nos testes

### 🔧 Correções e Ajustes

#### API do Flunt 2.0
- Atualizado `Notifiable` para `Notifiable<Notification>`
- Atualizado `Contract()` para `Contract<Notification>()`
- Alterado `Invalid` para `!IsValid`
- Corrigido tipos nullable em propriedades

#### API do MediatR 12.x
- Atualizado registro de serviços:
  ```csharp
  // Antes
  services.AddMediatR(Assembly.Load("..."));
  
  // Depois
  services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("...")));
  ```

### 📝 Documentação

#### README.md Completamente Reescrito
- ✅ Badges de tecnologias
- ✅ Descrição detalhada da arquitetura (Clean Architecture + DDD)
- ✅ Documentação de padrões utilizados (CQRS, Repository, Unit of Work)
- ✅ Instruções completas de instalação e execução
- ✅ Documentação da API com exemplos
- ✅ Explicação detalhada das regras de negócio
- ✅ Exemplos de cálculo de multas e juros
- ✅ Estrutura do projeto documentada
- ✅ Guia de contribuição

#### .gitignore Atualizado
- ✅ Padrão completo do Visual Studio 2022
- ✅ Suporte para VS Code
- ✅ Suporte para JetBrains Rider
- ✅ Arquivos de cobertura de código
- ✅ Arquivos temporários e de build

### 🧪 Testes

#### Cobertura Expandida
**Antes:** 7 testes  
**Depois:** 40 testes ✅

#### Novos Testes Adicionados

**Entidades (15 testes novos):**
- `PaidAccountTests` - 4 testes
- `PayableAccountTests` - 6 testes
- `PaymentRuleTests` - 5 testes

**Services (9 testes novos):**
- Testes de pagamento em dia
- Testes de atraso (1, 2, 3, 4, 5, 6, 7 dias)
- Testes com valores altos e baixos
- Testes de arredondamento
- Testes de pagamento antecipado

**Commands (5 testes novos):**
- Validação de nome vazio
- Validação de valor zero
- Validação de valor negativo
- Validação de data de vencimento
- Validação de data de pagamento

**Handlers (3 testes novos):**
- Teste de nome vazio
- Teste de valor zero
- Teste de retorno de dados

#### Melhorias nos Testes
- ✅ Migrado de `Assert.True()` para **FluentAssertions**
- ✅ Assertions mais legíveis e expressivas
- ✅ Melhor organização com padrão AAA (Arrange, Act, Assert)
- ✅ Campos readonly onde apropriado

### 📊 Resultados

```
Test Run Successful.
Total tests: 40
     Passed: 40 ✅
     Failed: 0
   Skipped: 0
 Total time: 0.5166 Seconds
```

### 🔄 Compatibilidade

- ✅ Projeto compila sem erros
- ✅ Todos os testes passando (40/40)
- ✅ Warnings de nullability documentados (não críticos)
- ✅ API REST funcional
- ✅ Swagger/OpenAPI atualizado

### 📦 Branch

As mudanças foram commitadas na branch: **`upgrade-dotnet10`**

Para criar um Pull Request:
```bash
https://github.com/gregojoao/PayableAccounts/pull/new/upgrade-dotnet10
```

### 🎯 Próximos Passos Sugeridos

1. Revisar e fazer merge da branch `upgrade-dotnet10`
2. Considerar adicionar testes de integração
3. Implementar endpoint de listagem de contas (mencionado no README original)
4. Adicionar logging estruturado (Serilog)
5. Implementar health checks
6. Adicionar Docker support
7. Configurar CI/CD pipeline

---

**Desenvolvido com ❤️ usando .NET 10**
