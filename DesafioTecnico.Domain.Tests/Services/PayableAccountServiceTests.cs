using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DesafioTecnico.Domain.Entities;
using DesafioTecnico.Domain.Services;
using DesafioTecnico.Domain.Services.Contracts;
using DesafioTecnico.Domain.Tests.FakeRepositories;
using FluentAssertions;
using Xunit;

namespace DesafioTecnico.Domain.Tests.Services
{
    public class PayableAccountServiceTests
    {
        private readonly IPayableAccountService _payableAccountService;

        public PayableAccountServiceTests()
        {
            var paymentRules = new List<PaymentRule>()
            {
                new PaymentRule(delayedDays: 0, finePercentage: 2, finePercentageInterestPerDay: 0.1m),
                new PaymentRule(delayedDays: 4, finePercentage: 3, finePercentageInterestPerDay: 0.2m),
                new PaymentRule(delayedDays: 6, finePercentage: 5, finePercentageInterestPerDay: 0.3m)
            };
            _payableAccountService = new PayableAccountService(new PaymentRuleFakeRepository(paymentRules));
        }

        [Fact]
        public async Task DadaUmaContaAPagarEOPagamentoDaMesmaEmDiaOValorPagoDeveSerIgualAoOriginal()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 100m, dueDate: DateTime.Now.Date.AddDays(3));
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date);
            
            paidAccount.AmountPaid.Should().Be(100m);
            paidAccount.DelayedDays.Should().Be(0);
        }

        [Fact]
        public async Task DadaUmaContaAPagarEOPagamentoDaMesmaComTresDiasDeAtrasoOValorPagoDeveSerDeCentoEDoisComTrinta()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 100m, dueDate: DateTime.Now.Date);
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date.AddDays(3));
            
            paidAccount.AmountPaid.Should().Be(102.30m);
            paidAccount.DelayedDays.Should().Be(3);
        }

        [Fact]
        public async Task DadaUmaContaAPagarEOPagamentoDaMesmaComCincoDiasDeAtrasoOValorPagoDeveSerDeCentoEQuatro()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 100m, dueDate: DateTime.Now.Date);
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date.AddDays(5));
            
            paidAccount.AmountPaid.Should().Be(104m);
            paidAccount.DelayedDays.Should().Be(5);
        }

        [Fact]
        public async Task DadaUmaContaAPagarEOPagamentoDaMesmaComSeisDiasDeAtrasoOValorPagoDeveSerDeCentoEOitoComOitenta()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 100m, dueDate: DateTime.Now.Date);
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date.AddDays(6));
            
            paidAccount.AmountPaid.Should().Be(106.80m);
            paidAccount.DelayedDays.Should().Be(6);
        }

        [Fact]
        public async Task DadaUmaContaAPagarEOPagamentoDaMesmaComUmDiaDeAtrasoDeveAplicarMultaDe2Porcento()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 100m, dueDate: DateTime.Now.Date);
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date.AddDays(1));
            
            paidAccount.AmountPaid.Should().Be(102.10m);
            paidAccount.DelayedDays.Should().Be(1);
        }

        [Fact]
        public async Task DadaUmaContaAPagarEOPagamentoDaMesmaComDoisDiasDeAtrasoDeveAplicarMultaDe2Porcento()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 100m, dueDate: DateTime.Now.Date);
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date.AddDays(2));
            
            paidAccount.AmountPaid.Should().Be(102.20m);
            paidAccount.DelayedDays.Should().Be(2);
        }

        [Fact]
        public async Task DadaUmaContaAPagarEOPagamentoDaMesmaComQuatroDiasDeAtrasoDeveAplicarMultaDe3Porcento()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 100m, dueDate: DateTime.Now.Date);
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date.AddDays(4));
            
            paidAccount.AmountPaid.Should().Be(103.80m);
            paidAccount.DelayedDays.Should().Be(4);
        }

        [Fact]
        public async Task DadaUmaContaAPagarEOPagamentoDaMesmaComSeteDiasDeAtrasoDeveAplicarMultaDe5Porcento()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 100m, dueDate: DateTime.Now.Date);
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date.AddDays(7));
            
            paidAccount.AmountPaid.Should().Be(107.10m);
            paidAccount.DelayedDays.Should().Be(7);
        }

        [Fact]
        public async Task DadaUmaContaAPagarComValorAltoEAtrasoDeveCalcularCorretamente()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 1000m, dueDate: DateTime.Now.Date);
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date.AddDays(6));
            
            paidAccount.AmountPaid.Should().Be(1068m);
            paidAccount.DelayedDays.Should().Be(6);
        }

        [Fact]
        public async Task DadaUmaContaAPagarComValorBaixoEAtrasoDeveCalcularCorretamente()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 10m, dueDate: DateTime.Now.Date);
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date.AddDays(3));
            
            paidAccount.AmountPaid.Should().Be(10.23m);
            paidAccount.DelayedDays.Should().Be(3);
        }

        [Fact]
        public async Task DadaUmaContaAPagarPagaAntesDoVencimentoNaoDeveAplicarMulta()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 100m, dueDate: DateTime.Now.Date.AddDays(10));
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date);
            
            paidAccount.AmountPaid.Should().Be(100m);
            paidAccount.DelayedDays.Should().Be(0);
        }

        [Fact]
        public async Task DadaUmaContaAPagarPagaNaDataDeVencimentoNaoDeveAplicarMulta()
        {
            var dueDate = DateTime.Now.Date;
            var payableAccount = new PayableAccount("Conta de teste", value: 100m, dueDate: dueDate);
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: dueDate);
            
            paidAccount.AmountPaid.Should().Be(100m);
            paidAccount.DelayedDays.Should().Be(0);
        }

        [Fact]
        public async Task DeveArredondarValorPagoParaDuasCasasDecimais()
        {
            var payableAccount = new PayableAccount("Conta de teste", value: 33.33m, dueDate: DateTime.Now.Date);
            var paidAccount = await _payableAccountService.Pay(payableAccount, payDay: DateTime.Now.Date.AddDays(3));
            
            // 33.33 + (33.33 * 0.02) + (33.33 * 0.003) = 33.33 + 0.6666 + 0.09999 = 34.09659 -> 34.10
            paidAccount.AmountPaid.Should().Be(34.10m);
        }
    }
}
