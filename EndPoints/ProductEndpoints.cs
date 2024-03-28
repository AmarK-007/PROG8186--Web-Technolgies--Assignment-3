using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Shoecart_ASP_Assignment3.Data;
using Shoecart_ASP_Assignment3.Models;
using System.Threading.Tasks;

namespace Shoecart_ASP_Assignment3.EndPoints {
    public static class ProductEndpoints {

        public static async Task<IActionResult> CreateProduct(Product newProduct, MyDBContext db) {
            db.Product.Add(newProduct);
            await db.SaveChangesAsync();

            var location = $"/api/Product/{newProduct.ProductId}";
            var responseContent = new {
                message = "Product created",
                product = newProduct
            };

            return new CreatedResult(location, responseContent);
        }

        public static async Task<IActionResult> UpdateProduct(int productId, Product updateProduct, MyDBContext db) {
            var product = await db.Product.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null) {
                return new JsonResult(new { message = "Product not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }

            product.Description = updateProduct.Description;
            product.ImageUrl = updateProduct.ImageUrl;
            product.Price = updateProduct.Price;
            product.ShippingCost = updateProduct.ShippingCost;

            await db.SaveChangesAsync();

            return new JsonResult(new { message = "Product updated" });
        }

        public static async Task<IActionResult> DeleteProduct(int productId, MyDBContext db) {
            var product = await db.Product.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null) {
                return new JsonResult(new { message = "Product not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }

            db.Product.Remove(product);
            await db.SaveChangesAsync();

            return new JsonResult(new { message = "Product deleted" });
        }

        public static void MapProductEndpoints(this IEndpointRouteBuilder routes) {
            var group = routes.MapGroup("/api/Product").WithTags(nameof(Product));

            group.MapGet("/", async (MyDBContext db) => {
                var products = await db.Product.ToListAsync();
                return new JsonResult(products);
            })
            .WithName("GetAllProducts")
            .Produces<Product[]>(StatusCodes.Status200OK);

            group.MapGet("/{productId}", async (int productId, MyDBContext db) => {
                var product = await db.Product.FirstOrDefaultAsync(p => p.ProductId == productId);
                if (product != null) {
                    return new JsonResult(product);
                } else {
                    return new JsonResult(new { message = "Product not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("GetProductById")
            .Produces<Product>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/{productId}", async (int productId, Product updateProduct, MyDBContext db) => {
                return await UpdateProduct(productId, updateProduct, db);
            })
            .WithName("UpdateProduct")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPost("/", async (Product newProduct, MyDBContext db) => {
                return await CreateProduct(newProduct, db);
            })
            .WithName("CreateProduct")
            .Produces<Product>(StatusCodes.Status201Created);

            group.MapDelete("/{productId}", async (int productId, MyDBContext db) => {
                return await DeleteProduct(productId, db);
            })
            .WithName("DeleteProduct")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
