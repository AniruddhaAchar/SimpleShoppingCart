using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ShoppingCartWeb.DataRepository;
using ShoppingCartWeb.Models;

namespace ShoppingCartTest;

[TestClass]
public class TestEFCartCollection
{
    private ShoppingCartContext _dbContext;
    private ILogger<ICartCollection> _logger;

    public required TestContext TestContext { get; set; }


    [TestInitialize]
    public void SetupDbContext()
    {
        var contextOptions = new DbContextOptionsBuilder<ShoppingCartContext>().UseInMemoryDatabase($"{TestContext.TestName}").EnableSensitiveDataLogging().Options;
        _dbContext = new ShoppingCartContext(contextOptions);
        _logger = Mock.Of<ILogger<ICartCollection>>();
    }

    [TestMethod]
    public void TestCreationOfCart()
    {
        EFCartCollection repo = new(_dbContext, _logger);
        repo.CreateCart(name: "foo");
        Assert.AreEqual(expected: 1, actual: _dbContext.Carts.Count());
        Assert.AreEqual(1, _dbContext.Carts.Where(cart => cart.Name == "foo").Count());
    }

    [TestMethod]
    public void TestDeletionOfCart()
    {
        EFCartCollection repo = new(_dbContext, _logger);
        var testCart = new Cart
        {
            Name = "bar"
        };
        _dbContext.Carts.Add(testCart);
        _dbContext.SaveChanges();

        repo.DeleteCart(testCart.CartId);

        var actualCart = _dbContext.Carts.Find(testCart.CartId);
        Assert.IsNull(actualCart);

        Assert.ThrowsException<KeyNotFoundException>(() => repo.DeleteCart(500));

    }

    [TestMethod]
    public void TestGetCart()
    {
        EFCartCollection repo = new(_dbContext, _logger);
        var testCart = new Cart
        {
            Name = "bar"
        };
        _dbContext.Carts.Add(testCart);
        _dbContext.SaveChanges();

        var actual = repo.GetCart(testCart.CartId);
        Assert.AreEqual(testCart, actual);

    }

    [TestMethod]
    public void TestGetAllCart()
    {
        EFCartCollection repo = new(_dbContext, _logger);
        var testCart = new Cart
        {
            Name = "bar"
        };
        var testCart2 = new Cart
        {
            Name = "foo"
        };
        _dbContext.Carts.Add(testCart);
        _dbContext.Carts.Add(testCart2);
        _dbContext.SaveChanges();

        var actual = repo.GetAllCarts();
        CollectionAssert.Contains(actual.ToList(), testCart);
        CollectionAssert.Contains(actual.ToList(), testCart2);
    }
}