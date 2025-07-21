using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Tests.Application.Sales
{
    public class UpdateSaleCommandHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly UpdateSaleCommandHandler _handler;

        public UpdateSaleCommandHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _handler = new UpdateSaleCommandHandler(_unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_DeveAtualizarVendaComSucesso_QuandoVendaExisteENaoEstaCancelada()
        {
            var saleId = Guid.NewGuid();
            var existingSale = new Sale { Id = saleId, Customer = "Cliente Antigo", Branch = "Filial Velha", IsCancelled = false };
            var command = new UpdateSaleCommand { Id = saleId, Customer = "Cliente Novo", Branch = "Filial Nova" };

            _unitOfWorkMock.Sales.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult<Sale?>(existingSale));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeTrue();
            existingSale.Customer.Should().Be("Cliente Novo");
            existingSale.Branch.Should().Be("Filial Nova");
            await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoVendaNaoExiste()
        {
            var saleId = Guid.NewGuid();
            var command = new UpdateSaleCommand { Id = saleId, Customer = "Cliente Novo", Branch = "Filial Nova" };

            _unitOfWorkMock.Sales.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult<Sale?>(null));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeFalse();
            await _unitOfWorkMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoVendaJaEstaCancelada()
        {
            var saleId = Guid.NewGuid();
            var existingSale = new Sale { Id = saleId, Customer = "Cliente Antigo", IsCancelled = true };
            var command = new UpdateSaleCommand { Id = saleId, Customer = "Cliente Novo", Branch = "Filial Nova" };

            _unitOfWorkMock.Sales.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                           .Returns(Task.FromResult<Sale?>(existingSale));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeFalse();
            await _unitOfWorkMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
