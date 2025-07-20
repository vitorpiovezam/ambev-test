using Ambev.DeveloperEvaluation.Common.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class ApplicationModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        builder.Services.AddScoped<CreateSaleUseCase>()

        builder.Services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(Ambev.DeveloperEvaluation.Application.Sales.CreateSale.CreateSaleCommand).Assembly));
    }
}