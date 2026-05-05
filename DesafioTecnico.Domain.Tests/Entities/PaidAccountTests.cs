using System;
using DesafioTecnico.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace DesafioTecnico.Domain.Tests.Entities
{
    public class PaidAccountTests
    {
        [Fact]
        public void DeveCriarContaPagaComDadosValidos()
        {
            // Arrange
            var payableAccountId = Guid.NewGuid();
            var payDay = DateTime.Now.Date;
            var delayedDays = 5;
            var amountPaid = 105.50m;

            // Act
            var paidAccount = new PaidAccount(payableAccountId, payDay, delayedDays, amountPaid);

            // Assert
            paidAccount.Should().NotBeNull();
            paidAccount.PayableAccountId.Should().Be(payableAccountId);
            paidAccount.PayDay.Should().Be(payDay);
            paidAccount.DelayedDays.Should().Be(delayedDays);
            paidAccount.AmountPaid.Should().Be(amountPaid);
            paidAccount.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void DeveCriarContaPagaSemAtraso()
        {
            // Arrange
            var payableAccountId = Guid.NewGuid();
            var payDay = DateTime.Now.Date;
            var delayedDays = 0;
            var amountPaid = 100m;

            // Act
            var paidAccount = new PaidAccount(payableAccountId, payDay, delayedDays, amountPaid);

            // Assert
            paidAccount.DelayedDays.Should().Be(0);
            paidAccount.AmountPaid.Should().Be(amountPaid);
        }

        [Fact]
        public void DeveCriarContaPagaComAtrasoMaximo()
        {
            // Arrange
            var payableAccountId = Guid.NewGuid();
            var payDay = DateTime.Now.Date;
            var delayedDays = 365;
            var amountPaid = 500m;

            // Act
            var paidAccount = new PaidAccount(payableAccountId, payDay, delayedDays, amountPaid);

            // Assert
            paidAccount.DelayedDays.Should().Be(delayedDays);
            paidAccount.AmountPaid.Should().BeGreaterThan(0);
        }

        [Fact]
        public void DeveArmazenarValorPagoComPrecisaoDecimal()
        {
            // Arrange
            var payableAccountId = Guid.NewGuid();
            var payDay = DateTime.Now.Date;
            var delayedDays = 3;
            var amountPaid = 102.30m;

            // Act
            var paidAccount = new PaidAccount(payableAccountId, payDay, delayedDays, amountPaid);

            // Assert
            paidAccount.AmountPaid.Should().Be(102.30m);
        }
    }
}
