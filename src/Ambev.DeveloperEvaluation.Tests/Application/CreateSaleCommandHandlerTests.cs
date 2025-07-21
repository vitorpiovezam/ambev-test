using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Tests.Application.Sales
{
    public class CreateSaleCommandHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly ILogger<CreateSaleCommandHandler> _loggerMock;
        private readonly CreateSaleCommandHandler _handler;
        private readonly Faker _faker;

        public CreateSaleCommandHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _loggerMock = Substitute.For<ILogger<CreateSaleCommandHandler>>();
            _faker = new Faker("pt_BR");

            _handler = new CreateSaleCommandHandler(_unitOfWorkMock, _loggerMock);
        }

        [Fact]
        public async Task Handle_DeveAplicar20PorcentoDeDesconto_QuandoQuantidadeFor10OuMais()
        {
            var command = new CreateSaleCommand
            {
                Customer = _faker.Person.FullName,
                Branch = "Filial de Teste",
                Items = new List<SaleItemCommand>
                {
                    new() { Product = "Cerveja IPA", Quantity = 15, UnitPrice = 10.00m }
                }
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            var expectedTotal = (15 * 10.00m) * 0.80m;
            result.TotalAmount.Should().Be(expectedTotal);
        }

        [Fact]
        public async Task Handle_DeveAplicar10PorcentoDeDesconto_QuandoQuantidadeEstiverEntre4e9()
        {
            var command = new CreateSaleCommand
            {
                Customer = _faker.Person.FullName,
                Branch = _faker.Company.CompanyName(),
                Items = new List<SaleItemCommand>
                {
                    new()
                    {
                        Product = _faker.Commerce.ProductName(),
                        Quantity = _faker.Random.Int(4, 9),
                        UnitPrice = _faker.Random.Decimal(5, 50)
                    }
                }
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            var item = command.Items.First();
            var expectedTotal = (item.Quantity * item.UnitPrice) * 0.90m;
            result.TotalAmount.Should().Be(expectedTotal);
        }

        [Fact]
        public async Task Handle_NaoDeveAplicarDesconto_QuandoQuantidadeForMenorQue4()
        {
            var command = new CreateSaleCommand
            {
                Customer = _faker.Person.FullName,
                Branch = "Filial de Teste",
                Items = new List<SaleItemCommand>
                {
                    new() { Product = "Agua Mineral", Quantity = 3, UnitPrice = 10.00m }
                }
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            var expectedTotal = 3 * 10.00m;
            result.TotalAmount.Should().Be(expectedTotal);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoQuantidadeForMaiorQue20()
        {
            var command = new CreateSaleCommand
            {
                Customer = _faker.Person.FullName,
                Branch = "Filial de Teste",
                Items = new List<SaleItemCommand>
                {
                    new() { Product = "Cerveja Lager", Quantity = 21, UnitPrice = 10.00m }
                }
            };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None)
            );
        }
    }
}
