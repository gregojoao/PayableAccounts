using DesafioTecnico.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace DesafioTecnico.Domain.Tests.Entities
{
    public class PaymentRuleTests
    {
        [Fact]
        public void DeveCriarRegraDeMultaComDadosValidos()
        {
            // Arrange
            var delayedDays = 3;
            var finePercentage = 2m;
            var finePercentageInterestPerDay = 0.1m;

            // Act
            var paymentRule = new PaymentRule(delayedDays, finePercentage, finePercentageInterestPerDay);

            // Assert
            paymentRule.Should().NotBeNull();
            paymentRule.DelayedDays.Should().Be(delayedDays);
            paymentRule.FinePercentage.Should().Be(finePercentage);
            paymentRule.FinePercentageInterestPerDay.Should().Be(finePercentageInterestPerDay);
            paymentRule.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void DeveCriarRegraPara3DiasDeAtraso()
        {
            // Arrange & Act
            var paymentRule = new PaymentRule(delayedDays: 0, finePercentage: 2m, finePercentageInterestPerDay: 0.1m);

            // Assert
            paymentRule.DelayedDays.Should().Be(0);
            paymentRule.FinePercentage.Should().Be(2m);
            paymentRule.FinePercentageInterestPerDay.Should().Be(0.1m);
        }

        [Fact]
        public void DeveCriarRegraPara4A5DiasDeAtraso()
        {
            // Arrange & Act
            var paymentRule = new PaymentRule(delayedDays: 4, finePercentage: 3m, finePercentageInterestPerDay: 0.2m);

            // Assert
            paymentRule.DelayedDays.Should().Be(4);
            paymentRule.FinePercentage.Should().Be(3m);
            paymentRule.FinePercentageInterestPerDay.Should().Be(0.2m);
        }

        [Fact]
        public void DeveCriarRegraPara6OuMaisDiasDeAtraso()
        {
            // Arrange & Act
            var paymentRule = new PaymentRule(delayedDays: 6, finePercentage: 5m, finePercentageInterestPerDay: 0.3m);

            // Assert
            paymentRule.DelayedDays.Should().Be(6);
            paymentRule.FinePercentage.Should().Be(5m);
            paymentRule.FinePercentageInterestPerDay.Should().Be(0.3m);
        }

        [Fact]
        public void DevePermitirMultaZero()
        {
            // Arrange & Act
            var paymentRule = new PaymentRule(delayedDays: 0, finePercentage: 0m, finePercentageInterestPerDay: 0m);

            // Assert
            paymentRule.FinePercentage.Should().Be(0m);
            paymentRule.FinePercentageInterestPerDay.Should().Be(0m);
        }
    }
}
