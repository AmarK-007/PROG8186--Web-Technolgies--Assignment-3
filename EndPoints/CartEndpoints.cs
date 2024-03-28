using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Shoecart_ASP_Assignment3.Data;
using Shoecart_ASP_Assignment3.Models;
using Microsoft.AspNetCore.Mvc;
namespace Shoecart_ASP_Assignment3.EndPoints;

public static class CartEndpoints {
    public static void MapCartEndpoints(this IEndpointRouteBuilder routes) {
        var group = routes.MapGroup("/api/Cart").WithTags(nameof(Cart));

        group.MapGet("/", async (MyDBContext db) => {
            var carts = await db.Cart.ToListAsync();
            return new JsonResult(new { message = "Retrieved carts", data = carts }) { StatusCode = StatusCodes.Status200OK };
        })
        .WithName("GetAllCarts")
        .WithOpenApi();

        group.MapGet("/{id}", async (int cartid, MyDBContext db) => {
            var cart = await db.Cart.AsNoTracking().FirstOrDefaultAsync(model => model.CartId == cartid);
            if (cart == null) {
                return new JsonResult(new { message = "Cart not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }
            return new JsonResult(new { message = "Retrieved cart", data = cart }) { StatusCode = StatusCodes.Status200OK };
        })
        .WithName("GetCartById")
        .WithOpenApi();



        group.MapPost("/", async (Cart cart, MyDBContext db) => {
            db.Cart.Add(cart);
            await db.SaveChangesAsync();
            return new JsonResult(new { message = "Created cart", data = cart }) { StatusCode = StatusCodes.Status201Created };
        })
        .WithName("CreateCart")
        .WithOpenApi();

        group.MapPut("/{id}", async (int cartid, Cart cart, MyDBContext db) => {
            var existingCart = await db.Cart.FirstOrDefaultAsync(m => m.CartId == cartid);
            if (existingCart != null) {
                existingCart.UserId = cart.UserId;
                await db.SaveChangesAsync();
                return new JsonResult(new { message = "Updated cart", data = existingCart }) { StatusCode = StatusCodes.Status200OK };
            } else {
                return new JsonResult(new { message = "Cart not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }
        })
        .WithName("UpdateCart")
        .WithOpenApi();

        group.MapDelete("/{id}", async (int cartid, MyDBContext db) => {
            var cart = await db.Cart.FirstOrDefaultAsync(m => m.CartId == cartid);
            if (cart != null) {
                db.Cart.Remove(cart);
                await db.SaveChangesAsync();
                return new JsonResult(new { message = "Deleted cart" }) { StatusCode = StatusCodes.Status200OK };
            } else {
                return new JsonResult(new { message = "Cart not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }
        })
        .WithName("DeleteCart")
        .WithOpenApi();
    }
}