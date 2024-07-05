using Microsoft.AspNetCore.Mvc;
using ShoppingCartWeb.DataRepository;
using ShoppingCartWeb.Models;

namespace ShoppingCartWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoppingCartController(ICartCollection cartCollection, ILogger<ShoppingCartController> logger) : ControllerBase
{
    [HttpGet]
    public IEnumerable<Cart> GetAll()
    {
        return cartCollection.GetAllCarts();
    }

    [HttpGet("{cartId}")]
    public ActionResult<Cart> GetCart(int cartId)
    {
        try
        {
            return cartCollection.GetCart(cartId);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public ActionResult<CartItem> CreateCart(Cart cart)
    {
        var cartId = cartCollection.CreateCart(cart.Name);
        return CreatedAtAction(nameof(GetCart), new { cartId = cartId }, new Cart { Name = cart.Name, CartId = cartId });
    }

    [HttpDelete]
    public ActionResult DeleteCart(Cart cart)
    {
        cartCollection.DeleteCart(cart.CartId);
        return NoContent();
    }

    [HttpGet("{cartId}/items")]
    public ActionResult<IEnumerable<CartItem>> GetAllCartItems([FromRoute] int cartId)
    {
        try
        {
            var items = cartCollection.GetItemCollection(cartId).GetAllItems();
            return Ok(items);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{cartId}/items/{itemId}")]
    public ActionResult<CartItem> GetCartItem([FromRoute] int cartId, [FromRoute] int itemId)
    {
        try
        {
            return cartCollection.GetItemCollection(cartId).GetCartItem(itemId);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{cartId}/items/")]
    public ActionResult UpdateCartItem([FromRoute] int cartId, [FromBody] CartItem cartItem)
    {
        try
        {
            cartCollection.GetItemCollection(cartId).UpdateCartItem(cartItem);
            return CreatedAtAction(nameof(GetCartItem), new { cartId = cartId, itemId = cartItem.CartItemId }, cartItem);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{cartId}/items/")]
    [Consumes("application/json")]
    public ActionResult AddCartItem([FromRoute] int cartId, [FromBody] CartItem cartItem)
    {
        try
        {
            var itemId = cartCollection.GetItemCollection(cartId).AddCartItem(cartItem);
            return CreatedAtAction(nameof(GetCartItem), new { cartId = cartId, itemId = itemId }, new CartItem
            {
                CartItemId = itemId,
                Name = cartItem.Name,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{cartId}/items/{itemId}")]
    public ActionResult AddCartItem([FromRoute] int cartId, [FromRoute] int itemId)
    {
        try
        {
            cartCollection.GetItemCollection(cartId).RemoveCartItem(itemId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

