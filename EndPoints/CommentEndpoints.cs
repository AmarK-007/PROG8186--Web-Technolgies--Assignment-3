using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Shoecart_ASP_Assignment3.Data;
using Shoecart_ASP_Assignment3.Models;
namespace Shoecart_ASP_Assignment3.EndPoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Comment").WithTags(nameof(Comment));

        group.MapGet("/", async (MyDBContext db) =>
        {
            return await db.Comments.ToListAsync();
        })
        .WithName("GetAllComments")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Comment>, NotFound>> (int commentid, MyDBContext db) =>
        {
            return await db.Comments.AsNoTracking()
                .FirstOrDefaultAsync(model => model.CommentId == commentid)
                is Comment model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCommentById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int commentid, Comment comment, MyDBContext db) =>
        {
            var affected = await db.Comments
                .Where(model => model.CommentId == commentid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.CommentId, comment.CommentId)
                    .SetProperty(m => m.ProductId, comment.ProductId)
                    .SetProperty(m => m.UserId, comment.UserId)
                    .SetProperty(m => m.Rating, comment.Rating)
                    .SetProperty(m => m.Image, comment.Image)
                    .SetProperty(m => m.Text, comment.Text)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateComment")
        .WithOpenApi();

        group.MapPost("/", async (Comment comment, MyDBContext db) =>
        {
            db.Comments.Add(comment);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Comment/{comment.CommentId}",comment);
        })
        .WithName("CreateComment")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int commentid, MyDBContext db) =>
        {
            var affected = await db.Comments
                .Where(model => model.CommentId == commentid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteComment")
        .WithOpenApi();
    }
}
