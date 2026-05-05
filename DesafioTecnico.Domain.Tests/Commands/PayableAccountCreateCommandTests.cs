using System;
using System.Collections.Generic;
using DesafioTecnico.Domain.Commands;
using Flunt.Notifications;
using FluentAssertions;
using Xunit;

namespace DesafioTecnico.Domain.Tests.Commands
{
    public class PayableAccountCreateCommandTests
    {
        private readonly PayableAccountCreateCommand _payableAccountCreateValidCommand;
        private readonly PayableAccountCreateCommand _payableAccountCreateInvalidCommand;

        public PayableAccountCreateCommandTests()
        {
            _payableAccountCreateValidCommand = new PayableAccountCreateCommand(
                name: "Conta de teste",
                value: 10.99m,
                dueDate: DateTime.Now.Date.AddDays(4),
                payDay: DateTime.Now.Date);
            _payableAccountCreateInvalidCommand = new PayableAccountCreateCommand();
        }

        [Fact]
        public void DadoUmComandoValidoORetornoDeveSerVerdadeiro()
        {
            _payableAccountCreateValidCommand.Validate();
            _payableAccountCreateValidCommand.IsValid.Should().BeTrue();
        }

        [Fact]
        public void DadoUmComandoInvalidoORetornoDeveSerFalso()
        {
            _payableAccountCreateInvalidCommand.Validate();
            _payableAccountCreateInvalidCommand.IsValid.Should().BeFalse();
            ((IList<Notification>)_payableAccountCreateInvalidCommand.Notifications).Count.Should().Be(4);
        }

        [Fact]
        public void DadoUmComandoSemNomeDeveRetornarErro()
        {
            var command = new PayableAccountCreateCommand(
                name: "",
                value: 10.99m,
                dueDate: DateTime.Now.Date.AddDays(4),
                payDay: DateTime.Now.Date);

            command.Validate();
            command.IsValid.Should().BeFalse();
            command.Notifications.Should().Contain(n => n.Key == "Name");
        }

        [Fact]
        public void DadoUmComandoComValorZeroDeveRetornarErro()
        {
            var command = new PayableAccountCreateCommand(
                name: "Conta teste",
                value: 0m,
                dueDate: DateTime.Now.Date.AddDays(4),
                payDay: DateTime.Now.Date);

            command.Validate();
            command.IsValid.Should().BeFalse();
            command.Notifications.Should().Contain(n => n.Key == "Value");
        }

        [Fact]
        public void DadoUmComandoComValorNegativoDeveRetornarErro()
        {
            var command = new PayableAccountCreateCommand(
                name: "Conta teste",
                value: -10m,
                dueDate: DateTime.Now.Date.AddDays(4),
                payDay: DateTime.Now.Date);

            command.Validate();
            command.IsValid.Should().BeFalse();
            command.Notifications.Should().Contain(n => n.Key == "Value");
        }

        [Fact]
        public void DadoUmComandoSemDataDeVencimentoDeveRetornarErro()
        {
            var command = new PayableAccountCreateCommand(
                name: "Conta teste",
                value: 10.99m,
                dueDate: null,
                payDay: DateTime.Now.Date);

            command.Validate();
            command.IsValid.Should().BeFalse();
            command.Notifications.Should().Contain(n => n.Key == "Due date");
        }

        [Fact]
        public void DadoUmComandoSemDataDePagamentoDeveRetornarErro()
        {
            var command = new PayableAccountCreateCommand(
                name: "Conta teste",
                value: 10.99m,
                dueDate: DateTime.Now.Date.AddDays(4),
                payDay: null);

            command.Validate();
            command.IsValid.Should().BeFalse();
            command.Notifications.Should().Contain(n => n.Key == "Pay Day");
        }
    }
}
