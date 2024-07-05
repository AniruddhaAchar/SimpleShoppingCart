using ShoppingCartWeb.Models;

namespace ShoppingCartWeb.DataRepository;

/// <summary>
/// Repository interface for CRUD inside single shopping cart
/// </summary>
public interface IItemCollection
{
    /// <summary>
    /// Add a CartItem to the cart.
    /// </summary>
    /// <param name="item">The cart item to be added.</param>
    /// <returns>The ID of the item that was added.</returns>
    public int AddCartItem(CartItem item);
    /// <summary>
    /// Remove an item from cart using item ID.
    /// </summary>
    /// <param name="itemId">The ID of the item to be removed.</param>
    public void RemoveCartItem(int itemId);
    /// <summary>
    /// Update an item in the cart.
    /// </summary>
    /// <param name="cartItem">The cart item to be updated.</param>
    public void UpdateCartItem(CartItem cartItem);
    /// <summary>
    /// Get an item in the cart by ID.
    /// </summary>
    /// <param name="itemId">The ID of the item to be returned.</param>
    /// <returns>Cart Item</returns>
    public CartItem GetCartItem(int itemId);
    /// <summary>
    /// Get all items in a cart.
    /// </summary>
    /// <returns>Enumerable of all items in the cart.</returns>
    public IEnumerable<CartItem> GetAllItems();
}

/// <summary>
/// Repository interface for Creation of shopping carts.
/// </summary>
public interface ICartCollection
{
    /// <summary>
    /// Carte a new cart.
    /// </summary>
    /// <param name="name">Name of the cart.</param>
    /// <returns>The ID of the cart item created.</returns>
    public int CreateCart(string name);
    /// <summary>
    /// Delete a cart by ID.
    /// </summary>
    /// <param name="cartId">Cart ID to be deleted.</param>
    public void DeleteCart(int cartId);
    /// <summary>
    /// Get a specific cart by ID.
    /// </summary>
    /// <param name="cartId">ID of the cart.</param>
    /// <returns>The cart.</returns>
    public Cart GetCart(int cartId);
    /// <summary>
    /// Get all carts.
    /// </summary>
    /// <returns>Enumerable of all carts.</returns>
    public IEnumerable<Cart> GetAllCarts();

    public IItemCollection GetItemCollection(int cartId);
}