using ShoppingCartWeb.Models;

namespace ShoppingCartWeb.DataRepository;

/// <summary>
/// Repository interface for CRUD inside single shopping cart
/// </summary>
public interface IItemCollection
{
    public int AddCartItem(CartItem item);
    public void RemoveCartItem(int itemId);
    public void UpdateCartItem(CartItem cartItem);
    public CartItem GetCartItem(int itemId);

    public IEnumerable<CartItem> GetAllItems();
}

/// <summary>
/// Repository interface for Creation of shopping carts.
/// </summary>
public interface ICartCollection
{
    public int CreateCart(string name);
    public void DeleteCart(int cartId);

    public Cart GetCart(int cartId);

    public IEnumerable<Cart> GetAllCarts();

    public IItemCollection GetItemCollection(int cartId);
}