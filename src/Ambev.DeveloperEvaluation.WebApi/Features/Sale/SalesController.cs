using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
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
    }
}
