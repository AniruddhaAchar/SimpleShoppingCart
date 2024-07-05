using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShoppingCartWeb.Models;

public class Cart
{
    [Required]
    public required string Name { get; set; }
    public int CartId { get; set; }
    public virtual ICollection<CartItem> CartItems { get; } = [];
}

public class CartItem
{

    public int CartItemId { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required int Quantity { get; set; }
    [Required]
    public required float Price { get; set; }
    [JsonIgnore]
    public virtual int CartId { get; set; }
    [JsonIgnore]
    public virtual Cart? Cart { get; set; }

}
