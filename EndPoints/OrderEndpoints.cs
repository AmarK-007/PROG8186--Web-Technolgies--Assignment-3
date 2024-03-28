using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Shoecart_ASP_Assignment3.Data;
using Shoecart_ASP_Assignment3.Models;
using System.Threading.Tasks;

namespace Shoecart_ASP_Assignment3.EndPoints {
    public static class OrderEndpoints {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder routes) {
            var group = routes.MapGroup("/api/Order").WithTags(nameof(Order));

            group.MapGet("/", async (MyDBContext db) => {
                var orders = await db.Orders.ToListAsync();
                return new JsonResult(new { message = "Retrieved all orders", data = orders }) { StatusCode = StatusCodes.Status200OK };
            })
            .WithName("GetAllOrders")
            .WithOpenApi();

            group.MapGet("/{id}", async (int orderid, MyDBContext db) => {
                var order = await db.Orders.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.OrderId == orderid);
                return order != null
                    ? new JsonResult(new { message = "Retrieved order", data = order }) { StatusCode = StatusCodes.Status200OK }
                    : new JsonResult(new { message = "Order not found" }) { StatusCode = StatusCodes.Status404NotFound };
            })
            .WithName("GetOrderById")
            .WithOpenApi();

            group.MapPut("/{id}", async (int orderid, Order order, MyDBContext db) => {
                var existingOrder = await db.Orders
                    .FirstOrDefaultAsync(model => model.OrderId == orderid);
                if (existingOrder != null) {
                    existingOrder.UserId = order.UserId;
                    await db.SaveChangesAsync();
                    return new JsonResult(new { message = "Updated order" }) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new JsonResult(new { message = "Order not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("UpdateOrder")
            .WithOpenApi();

            group.MapPost("/", async (Order order, MyDBContext db) => {
                // If there are any OrderItems, add them to the order
                if (order.OrderItems != null) {
                    foreach (var item in order.OrderItems) {
                        item.Order = order; // Set the Order of the OrderItem to the Order
                    }
                }

                // Add the order to the context
                db.Orders.Add(order);

                // Save changes to the database
                await db.SaveChangesAsync();

                return new JsonResult(new { message = "Created order", data = order }) { StatusCode = StatusCodes.Status201Created };
            })
            .WithName("CreateOrder")
            .WithOpenApi();

            group.MapDelete("/{id}", async (int orderid, MyDBContext db) => {
                var existingOrder = await db.Orders
                    .FirstOrDefaultAsync(model => model.OrderId == orderid);
                if (existingOrder != null) {
                    db.Orders.Remove(existingOrder);
                    await db.SaveChangesAsync();
                    return new JsonResult(new { message = "Deleted order" }) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new JsonResult(new { message = "Order not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("DeleteOrder")
            .WithOpenApi();
        }
    }
}