using Ambev.DeveloperEvaluation.Application.Sales.GetSaleById;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Tests.Application.Sales
{
    public class GetSaleByIdQueryHandlerTests
    {
        private readonly ISaleRepository _saleRepositoryMock;
        private readonly GetSaleByIdQueryHandler _handler;

        public GetSaleByIdQueryHandlerTests()
        {
            _saleRepositoryMock = Substitute.For<ISaleRepository>();
            _handler = new GetSaleByIdQueryHandler(_saleRepositoryMock);
        }

        [Fact]
        public async Task Handle_DeveRetornarVenda_QuandoVendaExiste()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var sale = new Sale { Id = saleId, Customer = "Cliente Teste" };
            var query = new GetSaleByIdQuery { Id = saleId };

            _saleRepositoryMock.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Sale?>(sale));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(saleId);
            result?.Customer.Should().Be("Cliente Teste");
        }
    }
}
