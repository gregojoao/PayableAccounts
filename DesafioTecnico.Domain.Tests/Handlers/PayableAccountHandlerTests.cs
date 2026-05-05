using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DesafioTecnico.Domain.Commands;
using DesafioTecnico.Domain.Entities;
using DesafioTecnico.Domain.Handlers;
using DesafioTecnico.Domain.Services;
using DesafioTecnico.Domain.Tests.FakeRepositories;
using DesafioTecnico.Domain.Tests.FakeTransactions;
using Flunt.Notifications;
using FluentAssertions;
using Xunit;

namespace DesafioTecnico.Domain.Tests.Handlers
{
    public class PayableAccountHandlerTests
    {
        private readonly PayableAccountHandler _payableAccountHandler;

        public PayableAccountHandlerTests()
        {
            var paymentRules = new List<PaymentRule>()
            {
                new PaymentRule(delayedDays: 0, finePercentage: 2, finePercentageInterestPerDay: 0.1m),
                new PaymentRule(delayedDays: 4, finePercentage: 3, finePercentageInterestPerDay: 0.2m),
                new PaymentRule(delayedDays: 6, finePercentage: 5, finePercentageInterestPerDay: 0.3m)
            };
            _payableAccountHandler = new PayableAccountHandler(
                new PayableAccountFakeRepository(),
                new PaidAccountFakeRepository(),
                new PayableAccountService(new PaymentRuleFakeRepository(paymentRules)),
                new FakeUnitOfWork());
        }

        [Fact]
        public async Task DadoUmRegistroDeUmaContaAPagarEOPagamentoDestaContaEmDiaORetornoDeveSerQueFoiRegistradaEPaga()
        {
            var command = new PayableAccountCreateCommand(
                name: "Conta de teste",
                value: 100m,
                dueDate: DateTime.Now.Date,
                payDay: DateTime.Now.Date);
            var retorno = (CommandResult)await _payableAccountHandler.Handle(command);
            
            retorno.Sucess.Should().BeTrue();
            retorno.Message.Should().Be("Conta a pagar registrada");
        }

        [Fact]
        public async Task DadoUmRegistroDeUmaContaAPagarEOPagamentoDestaContaEmAtrasoORetornoDeveSerQueFoiRegistradaEPaga()
        {
            var command = new PayableAccountCreateCommand(
                name: "Conta de teste",
                value: 100m,
                dueDate: DateTime.Now.Date,
                payDay: DateTime.Now.Date.AddDays(4));
            var retorno = (CommandResult)await _payableAccountHandler.Handle(command);
            
            retorno.Sucess.Should().BeTrue();
            retorno.Message.Should().Be("Conta a pagar registrada");
        }

        [Fact]
        public async Task DadoUmRegistroDeUmaContaAPagarInvalidaORetornoDeveSerQueFoiNaoRegistrada()
        {
            var command = new PayableAccountCreateCommand();
            var retorno = (CommandResult)await _payableAccountHandler.Handle(command);
            
            retorno.Sucess.Should().BeFalse();
            retorno.Message.Should().Be("Dados incompletos");
            ((IList<Notification>)retorno.Notification).Count.Should().Be(4);
        }

        [Fact]
        public async Task DadoUmRegistroComNomeVazioDeveRetornarErro()
        {
            var command = new PayableAccountCreateCommand(
                name: "",
                value: 100m,
                dueDate: DateTime.Now.Date,
                payDay: DateTime.Now.Date);
            var retorno = (CommandResult)await _payableAccountHandler.Handle(command);
            
            retorno.Sucess.Should().BeFalse();
            retorno.Message.Should().Be("Dados incompletos");
        }

        [Fact]
        public async Task DadoUmRegistroComValorZeroDeveRetornarErro()
        {
            var command = new PayableAccountCreateCommand(
                name: "Conta teste",
                value: 0m,
                dueDate: DateTime.Now.Date,
                payDay: DateTime.Now.Date);
            var retorno = (CommandResult)await _payableAccountHandler.Handle(command);
            
            retorno.Sucess.Should().BeFalse();
            retorno.Message.Should().Be("Dados incompletos");
        }

        [Fact]
        public async Task DadoUmRegistroValidoDeveRetornarOsDadosDaContaCriada()
        {
            var command = new PayableAccountCreateCommand(
                name: "Conta de teste",
                value: 100m,
                dueDate: DateTime.Now.Date,
                payDay: DateTime.Now.Date);
            var retorno = (CommandResult)await _payableAccountHandler.Handle(command);
            
            retorno.Sucess.Should().BeTrue();
            retorno.Entity.Should().NotBeNull();
            retorno.Entity.Should().BeOfType<PayableAccount>();
        }
    }
}
