using Mapster;
using Microsoft.EntityFrameworkCore;
using Satoshi.Converters;
using Satoshi.DTO;
using Satoshi.Models;
using Satoshi.Services;

namespace Satoshi.Extensions;
public static class ApiEndPoints
{ 
    //private static IOrderService repo;
    public static void MapProductEndPoint(this IEndpointRouteBuilder routes, IServiceProvider services, IOrderService repo)
    {
        //using IServiceScope serviceScope = services.CreateScope();
        //IServiceProvider provider = serviceScope.ServiceProvider;

        //repo ??= provider.GetRequiredService<IOrderService>();
        
        routes.MapGet("/products/{id}", async (int Id) =>
        {
            if ((await repo.ProductRepository.LoadProduct(Id)) is Product prod)
            {                
                if (prod.Adapt<ProductDTO>() is ProductDTO mpp) return Results.Ok(mpp);
                else return Results.Problem("Entity flattening failed");
            }
            return Results.NotFound();
        }).WithName("GetProductById")
        .Produces<ProductDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        routes.MapGet("/products/collection({Ids})", async (ListBinder<int> Ids) =>
        {
            if ((await repo.ProductRepository.FindProducts(Ids.ToArray()).ToListAsync()) is List<Product> prods)
            {
                if (prods.Adapt<IEnumerable<ProductDTO>>() is IEnumerable<ProductDTO> mpp) return Results.Ok(mpp);
                else return Results.Problem("Entity flattening failed");
            }
            return Results.NotFound();
        }).WithName("GetProductByIds")
       .Produces<IEnumerable<ProductDTO>>(StatusCodes.Status200OK)
       .ProducesProblem(StatusCodes.Status400BadRequest);

        routes.MapGet("/products", async (string term, float? price, float? maxPrice, bool? showstats, int? skip, int? top) =>
        {

            if ((showstats ?? false) && (await repo.ProductRepository.FindProductStats(null, term, null, null, (price ?? 0), (maxPrice ?? 0), 
                (skip ?? 0), (top ?? 20))) is IEnumerable<ProductStat> stats)
            {
                return Results.Ok(stats);
            }
            else if ((await repo.ProductRepository.FindProducts(null, term, (price ?? 0), (maxPrice ?? 0), (skip ?? 0), (top ?? 20)).ToListAsync()) is List<Product> prods)
            {
                if (prods.Adapt<IEnumerable<ProductDTO>>() is IEnumerable<ProductDTO> mpp) return Results.Ok(mpp);
                else return Results.Problem("Entity flattening failed");
            }
            return Results.NotFound();
        }).WithName("GetProducts")
        .Produces<IEnumerable<ProductDTO>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        //routes.MapGet("/products/stats", async (int productId, string term, DateTime? startDate, DateTime? endDate) =>
        //{
        //    if ((await repo.ProductRepository.FindProductStats(productId, term, startDate, endDate, 0, null, 0, 50) is IEnumerable<ProductStat> prods))
        //    {
        //        return Results.Ok(prods); 
        //    }
        //    return Results.NotFound();
        //}).WithName("GetProductStats")
        //.Produces<IEnumerable<ProductStat>>(StatusCodes.Status200OK)
        //.ProducesProblem(StatusCodes.Status400BadRequest);

        routes.MapPost("/products", async (string ProductName, float Price) =>
        {
            if (!string.IsNullOrWhiteSpace(ProductName) && ProductName.Length > 3)
            {
                if (!(await repo.ProductRepository.ProductNameExists(ProductName)))
                {
                    var p = new Product { ProductName = ProductName, Price = Price };
                    repo.ProductRepository.CreateEntity(p);
                    await repo.SaveAsync();

                    return Results.RedirectToRoute($"/products/{p.Id}");
                }
                else return Results.Conflict();
            }
            else return Results.BadRequest();
        }).WithName("CreateProduct") //.Produces<CreateTableCode>(StatusCodes.Status201Created)
         
        .ProducesValidationProblem();

        routes.MapPut("/products/{id}/set-price", async (int id, float Price) =>
        {
            if ((await repo.ProductRepository.LoadProduct(id, TrackChanges: true)) is Product prod)
            {
                prod.Price = Price; //wud love to store old price. for future implementation
                await repo.SaveAsync();

                return Results.NoContent();
            }
            return Results.NotFound();
        }).WithName("UpdatePrice") //.Produces<CreateTableCode>(StatusCodes.Status201Created)
       .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        routes.MapDelete("/products/{id}", async (int id) =>
        {
            if ((await repo.ProductRepository.LoadProduct(id, TrackChanges: true)) is Product prod)
            {
                repo.ProductRepository.DeleteEntity(prod); //wud love to store old price. for future implementation
                await repo.SaveAsync();

                return Results.NoContent();
            }
            return Results.NotFound();
        }).WithName("DeleteProduct") //.Produces<CreateTableCode>(StatusCodes.Status201Created)
     .Produces(StatusCodes.Status204NoContent)
          .Produces(StatusCodes.Status404NotFound);
    }

    public static void MapOrderEndPoint(this IEndpointRouteBuilder routes, IServiceProvider services, IOrderService repo)
    {
        //using IServiceScope serviceScope = services.CreateScope();
        //IServiceProvider provider = serviceScope.ServiceProvider;

        //repo ??= provider.GetRequiredService<IOrderService>();

        routes.MapGet("/orders/{id}", async (int Id) =>
        {
            if ((await repo.OrderRepository.LoadOrder(Id)) is Order order)
            {
                if (order.Adapt<OrderDTO>() is OrderDTO mpp) return Results.Ok(mpp);
                else return Results.Problem("Entity flattening failed");
            }
            return Results.NotFound();
        }).WithName("GetOrderById")
        .Produces<OrderDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        routes.MapGet("/orders/collection({Ids})", async (ListBinder<int> Ids) =>
        {
            if ((await repo.OrderRepository.FindOrders(Ids.ToArray()).ToListAsync() is List<Order> prods))
            {
                if (prods.Adapt<IEnumerable<Order>>() is IEnumerable<OrderDTO> mpp) return Results.Ok(mpp);
                else return Results.Problem("Entity flattening failed");
            }
            return Results.NotFound();
        }).WithName("GetOrderByIds")
       .Produces<IEnumerable<OrderDTO>>(StatusCodes.Status200OK)
       .ProducesProblem(StatusCodes.Status400BadRequest);

        routes.MapGet("/orders", async (string term, string custName, int? productId, float? minPrice, float? maxPrice, int? skip, int? top) =>
        {
            if ((await repo.OrderRepository.FindOrders(null, productId ?? 0, custName, term, minPrice ?? 0, maxPrice ?? 0, skip ?? 0, top ?? 10).ToListAsync()) is List<Order> prods)
            {
                if (prods.Adapt<List<OrderDTO>>() is List<OrderDTO> mpp) return Results.Ok(mpp);
                else return Results.Problem("Entity flattening failed");
            }
            return Results.NotFound();
        }).WithName("GetOrders")
        .Produces<IEnumerable<OrderDTO>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
         
        routes.MapPost("/orders", async (int productId, string customer, bool? ignoreConflicts) =>
        {
            if (!string.IsNullOrWhiteSpace(customer) && customer.Length > 0)
            {
                if ((ignoreConflicts ?? false) || !(await repo.OrderRepository.OrderExists(customer, productId)))
                {
                    if ((await repo.ProductRepository.LoadProduct(productId)) is Product prod)
                    {
                        var o = new Order { CustomerName = customer, ProductId = productId, Price = prod.Price };
                    }
                    else return Results.NotFound("Product does not Exists");
                }
                return Results.Conflict();
            }
            return Results.BadRequest();
        }).WithName("CreateOrder") //.Produces<CreateTableCode>(StatusCodes.Status201Created)
         .ProducesValidationProblem();

       
        routes.MapDelete("/orders/{id}", async (int id) =>
        {
            if ((await repo.OrderRepository.LoadOrder(id, TrackChanges: true)) is Order order)
            {
                repo.OrderRepository.DeleteEntity(order);
                await repo.SaveAsync();
            } else return Results.NotFound();
            return Results.NoContent();
        }).WithName("DeleteOrder") //.Produces<CreateTableCode>(StatusCodes.Status201Created)
     .Produces(StatusCodes.Status204NoContent)
          .Produces(StatusCodes.Status404NotFound);
    }
}
