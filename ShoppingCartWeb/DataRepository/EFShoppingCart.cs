using System.Collections.Frozen;
using ShoppingCartWeb.Models;

namespace ShoppingCartWeb.DataRepository;

public class EFShoppingCart(ShoppingCartContext shoppingCartContext, ILogger<ICartCollection> logger, int shoppingCartId) : IItemCollection
{
    private readonly Cart _cart = shoppingCartContext.Carts.Find(shoppingCartId) ?? throw new KeyNotFoundException("No cart with given id");
    private readonly ILogger<ICartCollection> _logger = logger;

    private readonly ShoppingCartContext _shoppingCartContext = shoppingCartContext;

    public int AddCartItem(CartItem item)
    {
        try
        {
            _cart.CartItems.Add(item);
            _shoppingCartContext.SaveChanges();
            return item.CartItemId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception when saving item with id{itemId}", item.CartItemId);
            throw;
        }
    }

    public IEnumerable<CartItem> GetAllItems()
    {
        try
        {
            return _cart.CartItems.AsEnumerable();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception when trying to get all items");
            throw;
        }
    }

    public CartItem GetCartItem(int itemId)
    {
        try
        {


            return _cart.CartItems.Where(item => item.CartItemId == itemId).FirstOrDefault() ?? throw new KeyNotFoundException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception when retriving item with id {itemId}", itemId);
            throw;
        }
    }

    public void RemoveCartItem(int itemId)
    {
        try
        {
            var itemToRemove = _cart.CartItems.Where(item => item.CartItemId == itemId).FirstOrDefault() ?? throw new KeyNotFoundException();
            _cart.CartItems.Remove(itemToRemove);
            _shoppingCartContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception when deleting item with id {itemId}", itemId);
            throw;
        }

    }

    public void UpdateCartItem(CartItem cartItem)
    {
        try
        {
            var itemToUpdate = _cart.CartItems.Where(item => item.CartItemId == cartItem.CartItemId).FirstOrDefault() ?? throw new KeyNotFoundException();
            itemToUpdate.Name = cartItem.Name;
            itemToUpdate.Price = cartItem.Price;
            itemToUpdate.Quantity = cartItem.Quantity;
            _shoppingCartContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception when updating item with id {itemId}", cartItem.CartItemId);
            throw;
        }
    }
}

public class EFCartCollection(ShoppingCartContext _shoppingCartContext, ILogger<ICartCollection> _logger) : ICartCollection
{
    public int CreateCart(string name)
    {
        try
        {
            var newCart = new Cart
            {
                Name = name
            };
            _shoppingCartContext.Carts.Add(newCart);
            _shoppingCartContext.SaveChanges();
            return newCart.CartId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error when creating cart with name {name}", name);
            throw;
        }
    }

    public void DeleteCart(int cartId)
    {
        try
        {
            var cartToRemove = _shoppingCartContext.Carts.Find(cartId) ?? throw new KeyNotFoundException("Cart with given ID not found");
            _shoppingCartContext.Carts.Remove(cartToRemove);
            _shoppingCartContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error when removing cart with ID {cartId}", cartId);
            throw;
        }
    }

    public IEnumerable<Cart> GetAllCarts()
    {
        return _shoppingCartContext.Carts.AsEnumerable();
    }

    public Cart GetCart(int cartId)
    {
        return _shoppingCartContext.Carts.Find(cartId) ?? throw new KeyNotFoundException("Cart with given ID not found");
    }

    public IItemCollection GetItemCollection(int cartId)
    {
        return new EFShoppingCart(_shoppingCartContext, _logger, cartId);
    }
}
