using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Shoecart_ASP_Assignment3.Data;
using Shoecart_ASP_Assignment3.Models;
using System.Threading.Tasks;

namespace Shoecart_ASP_Assignment3.EndPoints {
    public static class CommentEndpoints {
        public static void MapCommentEndpoints(this IEndpointRouteBuilder routes) {
            var group = routes.MapGroup("/api/Comment").WithTags(nameof(Comment));

            group.MapGet("/", async (MyDBContext db) => {
                var comments = await db.Comments.ToListAsync();
                return new JsonResult(new { message = "Retrieved all comments", data = comments }) { StatusCode = StatusCodes.Status200OK };
            })
            .WithName("GetAllComments")
            .WithOpenApi();

            group.MapGet("/{id}", async (int commentid, MyDBContext db) => {
                var comment = await db.Comments.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.CommentId == commentid);
                return comment != null
                    ? new JsonResult(new { message = "Retrieved comment", data = comment }) { StatusCode = StatusCodes.Status200OK }
                    : new JsonResult(new { message = "Comment not found" }) { StatusCode = StatusCodes.Status404NotFound };
            })
            .WithName("GetCommentById")
            .WithOpenApi();

            group.MapPut("/{id}", async (int commentid, Comment comment, MyDBContext db) => {
                var existingComment = await db.Comments
                    .FirstOrDefaultAsync(model => model.CommentId == commentid);
                if (existingComment != null) {
                    existingComment.ProductId = comment.ProductId;
                    existingComment.UserId = comment.UserId;
                    existingComment.Rating = comment.Rating;
                    existingComment.Image = comment.Image;
                    existingComment.Text = comment.Text;
                    await db.SaveChangesAsync();
                    return new JsonResult(new { message = "Updated comment" }) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new JsonResult(new { message = "Comment not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("UpdateComment")
            .WithOpenApi();

            group.MapPost("/", async (Comment comment, MyDBContext db) => {
                var userExists = await db.User.AnyAsync(u => u.user_id == comment.UserId);
                if (!userExists) {
                    return new JsonResult(new { message = "User not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }

                db.Comments.Add(comment);
                await db.SaveChangesAsync();
                return new JsonResult(new { message = "Created comment", data = comment }) { StatusCode = StatusCodes.Status201Created };
            })
           .WithName("CreateComment")
           .WithOpenApi();

            group.MapDelete("/{id}", async (int commentid, MyDBContext db) => {
                var existingComment = await db.Comments
                    .FirstOrDefaultAsync(model => model.CommentId == commentid);
                if (existingComment != null) {
                    db.Comments.Remove(existingComment);
                    await db.SaveChangesAsync();
                    return new JsonResult(new { message = "Deleted comment" }) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new JsonResult(new { message = "Comment not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("DeleteComment")
            .WithOpenApi();
        }
    }
}