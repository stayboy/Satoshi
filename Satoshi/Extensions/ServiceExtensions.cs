using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Satoshi.Contexts;
using Satoshi.DTO;
using Satoshi.Models;
using Satoshi.Repository;
using Satoshi.Services;
using System.Reflection;

namespace Satoshi.Extensions;
public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(o =>
        {
            o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .WithMethods("POST", "GET")
                .AllowAnyHeader(); 
            });
        });
    }

    public static void ConfigureMapster(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

        TypeAdapterConfig<Product, ProductDTO>.NewConfig();
        TypeAdapterConfig<Order, OrderDTO>.NewConfig()
             .Map(dest => dest.ProductName, src => src.Product != null ? src.Product.ProductName : string.Empty); 

        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        // scans the assembly and gets the IRegister, adding the registration to the TypeAdapterConfig
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
        // register the mapper as Singleton service for my application

        var mapperConfig = new Mapper(typeAdapterConfig);
        services.AddSingleton<IMapper>(mapperConfig);
    }
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new()
            {
                Title = "Satoshi Enterprise DotNet Core",
                Version = "v1"
            });
        });
    }
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SatoContext>(opts =>
        {
            opts.UseSqlite($"Data Source=DbLite/Satoshi.db;Command Timeout=60"); 
        });
    }
    public static void ConfigureIISIntegration(this IServiceCollection services)
    {
        services.Configure<IISOptions>(o => { });
    }
    public static void ConfigureRepoServices(this IServiceCollection services)
    {        
        services.TryAddScoped<IOrderRepository, OrderRepository>();
        services.TryAddScoped<IProductRepository, ProductRepository>();
        services.TryAddScoped<IOrderService, OrderService>();
    }
}
