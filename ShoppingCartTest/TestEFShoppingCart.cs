
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Moq;
using ShoppingCartWeb.DataRepository;
using ShoppingCartWeb.Models;

namespace ShoppingCartTest;

[TestClass]
public class EFShoppingCartTest
{
    private ShoppingCartContext _dbContext;
    private ILogger<ICartCollection> _logger;

    private string _cartName1;
    private string _cartName2;

    private int _cartId1;
    private int _cartId2;
    public required TestContext TestContext { get; set; }


    [TestInitialize]
    public void SetupDbContext()
    {
        var contextOptions = new DbContextOptionsBuilder<ShoppingCartContext>().UseInMemoryDatabase($"{TestContext.TestName}").EnableSensitiveDataLogging().Options;
        _dbContext = new ShoppingCartContext(contextOptions);
        _cartName1 = "fooCart";
        _cartName2 = "barCart";
        var cart1 = new Cart
        {
            Name = _cartName1
        };
        var cart2 = new Cart
        {
            Name = _cartName2
        };
        _dbContext.Carts.Add(cart1);
        _dbContext.Carts.Add(cart2);
        _cartId1 = cart1.CartId;
        _cartId2 = cart2.CartId;
        _dbContext.SaveChanges();
        _logger = Mock.Of<ILogger<ICartCollection>>();
    }

    [TestMethod]
    public void TestAddToCart()
    {

        var repository = new EFShoppingCart(_dbContext, _logger, shoppingCartId: _cartId1);
        var cartItem = new CartItem
        {
            Name = "foo",
            Quantity = 1,
            Price = 1.1f,
        };
        var itemId = repository.AddCartItem(cartItem);
        Assert.AreEqual(_dbContext.CartItems.Count(), 1);
        var actual = _dbContext.CartItems.Where(item => item.CartItemId == itemId).FirstOrDefault<CartItem>();
        Assert.IsNotNull(actual);
        Assert.AreEqual(actual, cartItem);
        Assert.AreEqual(expected: 1, actual: _dbContext.Carts.Find(_cartId1)?.CartItems.Count);
    }

    [TestMethod]
    public void TestGetAllFromCart()
    {
        var expectedCartItems = new List<CartItem> {
            new() {
                Name = "foo",
                Quantity = 1,
                Price = 1.1f,
            },
            new() {
                Name = "bar",
                Quantity = 1,
                Price = 1.1f,
            },
        };
        _dbContext.Carts.Find(_cartId1)?.CartItems.Add(expectedCartItems[0]);
        _dbContext.Carts.Find(_cartId1)?.CartItems.Add(expectedCartItems[1]);
        _dbContext.SaveChanges();
        var repository = new EFShoppingCart(_dbContext, _logger, _cartId1);
        var actual = repository.GetAllItems();
        CollectionAssert.AreEquivalent(expectedCartItems, actual.ToList());
    }

    [TestMethod]
    public void TestGetItem()
    {
        var cartItems = new List<CartItem> {
            new() {
                Name = "foo",
                Quantity = 1,
                Price = 1.1f,
            },
            new() {
                Name = "bar",
                Quantity = 1,
                Price = 1.1f,
            },
        };
        _dbContext.Carts.Find(_cartId2)?.CartItems.Add(cartItems[0]);
        _dbContext.Carts.Find(_cartId2)?.CartItems.Add(cartItems[1]);
        _dbContext.SaveChanges();
        var repository = new EFShoppingCart(_dbContext, _logger, _cartId2);

        var actualItem1 = repository.GetCartItem(cartItems[0].CartItemId);
        Assert.AreEqual(expected: cartItems[0], actual: actualItem1);

        var actualItem2 = repository.GetCartItem(cartItems[1].CartItemId);
        Assert.AreEqual(expected: cartItems[1], actual: actualItem2);

        Assert.ThrowsException<KeyNotFoundException>(() => repository.GetCartItem(500));
    }

    [TestMethod]
    public void TestUpdateItem()
    {
        var cartItems = new List<CartItem> {
            new() {
                Name = "foo",
                Quantity = 1,
                Price = 1.1f,
            },
            new() {
                Name = "bar",
                Quantity = 1,
                Price = 1.1f,
            },
        };
        _dbContext.Carts.Find(_cartId1)?.CartItems.Add(cartItems[0]);
        _dbContext.Carts.Find(_cartId1)?.CartItems.Add(cartItems[1]);
        _dbContext.SaveChanges();
        var repository = new EFShoppingCart(_dbContext, _logger, _cartId1);
        CartItem updateItem = new()
        {
            Name = "foo-updated",
            CartItemId = cartItems[0].CartItemId,
            Quantity = 2,
            Price = 2.5f
        };

        CartItem nonExistingItem = new()
        {
            Name = "foo-bar",
            CartItemId = 500,
            Quantity = 2,
            Price = 500f
        };

        repository.UpdateCartItem(updateItem);
        var actual = _dbContext.CartItems.Where(item => item.CartItemId == cartItems[0].CartItemId).FirstOrDefault();
        Assert.IsNotNull(actual);
        Assert.AreEqual(actual: actual.Name, expected: updateItem.Name);
        Assert.AreEqual(actual: actual.Quantity, expected: updateItem.Quantity);
        Assert.AreEqual(actual: actual.Price, expected: updateItem.Price);

        Assert.ThrowsException<KeyNotFoundException>(() => repository.UpdateCartItem(nonExistingItem));

    }

    [TestMethod]
    public void TestDeleteItem()
    {
        var cartItems = new List<CartItem> {
            new() {
                Name = "foo",
                Quantity = 1,
                Price = 1.1f,
            },
            new() {
                Name = "bar",
                Quantity = 1,
                Price = 1.1f,
            },
        };
        _dbContext.Carts.Find(_cartId1)?.CartItems.Add(cartItems[0]);
        _dbContext.Carts.Find(_cartId1)?.CartItems.Add(cartItems[1]);
        _dbContext.SaveChanges();
        var repository = new EFShoppingCart(_dbContext, _logger, _cartId1);
        repository.RemoveCartItem(cartItems[1].CartItemId);
        var item = _dbContext.Carts.Find(_cartId1)?.CartItems.Where(item => item.CartItemId == cartItems[1].CartItemId).FirstOrDefault();
        Assert.IsNull(item);
        Assert.AreEqual(expected: 1, actual: _dbContext.Carts.Find(_cartId1)?.CartItems.Count);

        Assert.ThrowsException<KeyNotFoundException>(() => repository.RemoveCartItem(500));
    }
}