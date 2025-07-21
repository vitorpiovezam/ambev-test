using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Application.Sales.GetSaleById;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var command = _mapper.Map<CreateSaleCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponseWithData<CreateSaleResponse>
                {
                    Success = true,
                    Message = "Venda criada com sucesso.",
                    Data = _mapper.Map<CreateSaleResponse>(response)
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales(CancellationToken cancellationToken)
        {
            var query = new GetAllSalesQuery();
            var response = await _mediator.Send(query, cancellationToken);

            return Ok(new ApiResponseWithData<List<GetAllSalesQueryResponse>>
            {
                Success = true,
                Message = "Vendas recuperadas com sucesso.",
                Data = response
            });
        }
        
        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> CancelSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new CancelSaleCommand { SaleId = id };
            var success = await _mediator.Send(command, cancellationToken);

            if (!success)
            {
                return NotFound(new ApiResponse { Success = false, Message = "Venda não encontrada." });
            }
            
            return Ok(new ApiResponse { Success = true, Message = "Venda cancelada com sucesso." });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSaleById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var query = new GetSaleByIdQuery { Id = id };
            var response = await _mediator.Send(query, cancellationToken);

            if (response is null)
            {
                return NotFound(new ApiResponse { Success = false, Message = "Venda não encontrada." });
            }
            
            return Ok(new ApiResponseWithData<GetSaleByIdQueryResponse>
            {
                Success = true,
                Message = "Venda recuperada com sucesso.",
                Data = response
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateSaleCommand 
            { 
                Id = id,
                Customer = request.Customer,
                Branch = request.Branch
            };
            var success = await _mediator.Send(command, cancellationToken);

            if (!success)
            {
                return NotFound(new ApiResponse { Success = false, Message = "Venda não encontrada ou já cancelada." });
            }
            
            return Ok(new ApiResponse { Success = true, Message = "Venda atualizada com sucesso." });
        }
    }
}
