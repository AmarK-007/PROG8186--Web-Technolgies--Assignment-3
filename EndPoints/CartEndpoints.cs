using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Shoecart_ASP_Assignment3.Data;
using Shoecart_ASP_Assignment3.Models;
namespace Shoecart_ASP_Assignment3.EndPoints;

public static class CartEndpoints
{
    public static void MapCartEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Cart").WithTags(nameof(Cart));

        group.MapGet("/", async (MyDBContext db) =>
        {
            return await db.Cart.ToListAsync();
        })
        .WithName("GetAllCarts")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cart>, NotFound>> (int cartid, MyDBContext db) =>
        {
            return await db.Cart.AsNoTracking()
                .FirstOrDefaultAsync(model => model.CartId == cartid)
                is Cart model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCartById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int cartid, Cart cart, MyDBContext db) =>
        {
            var affected = await db.Cart
                .Where(model => model.CartId == cartid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.CartId, cart.CartId)
                    .SetProperty(m => m.UserId, cart.UserId)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCart")
        .WithOpenApi();

        group.MapPost("/", async (Cart cart, MyDBContext db) =>
        {
            db.Cart.Add(cart);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cart/{cart.CartId}",cart);
        })
        .WithName("CreateCart")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int cartid, MyDBContext db) =>
        {
            var affected = await db.Cart
                .Where(model => model.CartId == cartid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCart")
        .WithOpenApi();
    }
}
