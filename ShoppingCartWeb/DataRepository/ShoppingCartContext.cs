namespace ShoppingCartWeb.DataRepository;
using Microsoft.EntityFrameworkCore;
using ShoppingCartWeb.Models;

public class ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : DbContext(options)
{
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
}