using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Tests.Application.Sales
{
    public class CancelSaleCommandHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly ILogger<CancelSaleCommandHandler> _loggerMock;
        private readonly CancelSaleCommandHandler _handler;

        public CancelSaleCommandHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _loggerMock = Substitute.For<ILogger<CancelSaleCommandHandler>>();
            _handler = new CancelSaleCommandHandler(_unitOfWorkMock, _loggerMock);
        }

        [Fact]
        public async Task Handle_DeveCancelarVendaComSucesso_QuandoVendaExiste()
        {
            var saleId = Guid.NewGuid();
            var existingSale = new Sale { Id = saleId, IsCancelled = false };
            var command = new CancelSaleCommand { SaleId = saleId };

            _unitOfWorkMock.Sales.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult<Sale?>(existingSale));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeTrue();
            existingSale.IsCancelled.Should().BeTrue();
            await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoVendaNaoExiste()
        {
            var saleId = Guid.NewGuid();
            var command = new CancelSaleCommand { SaleId = saleId };

            _unitOfWorkMock.Sales.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult<Sale?>(null));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeFalse();
            await _unitOfWorkMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
