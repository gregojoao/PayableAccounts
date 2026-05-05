using System;
using DesafioTecnico.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace DesafioTecnico.Domain.Tests.Entities
{
    public class PayableAccountTests
    {
        [Fact]
        public void DeveCriarContaAPagarComDadosValidos()
        {
            // Arrange
            var name = "Conta de Teste";
            var value = 150.50m;
            var dueDate = DateTime.Now.Date.AddDays(5);

            // Act
            var payableAccount = new PayableAccount(name, value, dueDate);

            // Assert
            payableAccount.Should().NotBeNull();
            payableAccount.Name.Should().Be(name);
            payableAccount.Value.Should().Be(value);
            payableAccount.DueDate.Should().Be(dueDate);
            payableAccount.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void DevePermitirCriarContaComValorMinimo()
        {
            // Arrange
            var name = "Conta Mínima";
            var value = 0.01m;
            var dueDate = DateTime.Now.Date;

            // Act
            var payableAccount = new PayableAccount(name, value, dueDate);

            // Assert
            payableAccount.Value.Should().Be(value);
        }

        [Fact]
        public void DevePermitirCriarContaComValorAlto()
        {
            // Arrange
            var name = "Conta Alta";
            var value = 999999.99m;
            var dueDate = DateTime.Now.Date;

            // Act
            var payableAccount = new PayableAccount(name, value, dueDate);

            // Assert
            payableAccount.Value.Should().Be(value);
        }

        [Fact]
        public void DevePermitirDataDeVencimentoNoPassado()
        {
            // Arrange
            var name = "Conta Vencida";
            var value = 100m;
            var dueDate = DateTime.Now.Date.AddDays(-10);

            // Act
            var payableAccount = new PayableAccount(name, value, dueDate);

            // Assert
            payableAccount.DueDate.Should().Be(dueDate);
            payableAccount.DueDate.Should().BeBefore(DateTime.Now.Date);
        }

        [Fact]
        public void DevePermitirDataDeVencimentoNoFuturo()
        {
            // Arrange
            var name = "Conta Futura";
            var value = 100m;
            var dueDate = DateTime.Now.Date.AddDays(30);

            // Act
            var payableAccount = new PayableAccount(name, value, dueDate);

            // Assert
            payableAccount.DueDate.Should().Be(dueDate);
            payableAccount.DueDate.Should().BeAfter(DateTime.Now.Date);
        }
    }
}
