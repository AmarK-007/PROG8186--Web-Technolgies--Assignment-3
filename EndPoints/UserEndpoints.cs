using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Shoecart_ASP_Assignment3.Data;
using Shoecart_ASP_Assignment3.Models;
namespace Shoecart_ASP_Assignment3.EndPoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/User").WithTags(nameof(User));

        group.MapGet("/", async (MyDBContext db) =>
        {
            return await db.User.ToListAsync();
        })
        .WithName("GetAllUsers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<User>, NotFound>> (int user_id, MyDBContext db) =>
        {
            return await db.User.AsNoTracking()
                .FirstOrDefaultAsync(model => model.user_id == user_id)
                is User model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUserById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int user_id, User user, MyDBContext db) =>
        {
            var affected = await db.User
                .Where(model => model.user_id == user_id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.user_id, user.user_id)
                    .SetProperty(m => m.name, user.name)
                    .SetProperty(m => m.email, user.email)
                    .SetProperty(m => m.password, user.password)
                    .SetProperty(m => m.username, user.username)
                    .SetProperty(m => m.purchase_history, user.purchase_history)
                    .SetProperty(m => m.shipping_address, user.shipping_address)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUser")
        .WithOpenApi();

        group.MapPost("/", async (User user, MyDBContext db) =>
        {
            db.User.Add(user);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/User/{user.user_id}",user);
        })
        .WithName("CreateUser")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int user_id, MyDBContext db) =>
        {
            var affected = await db.User
                .Where(model => model.user_id == user_id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUser")
        .WithOpenApi();
    }
}
