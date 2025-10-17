using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using web_api_with_ef_core_odata.Data;
using web_api_with_ef_core_odata.Services;
using web_api_with_ef_core_odata.Models;

namespace web_api_with_ef_core_odata.Tests;
public class ProductServiceTests : IDisposable
{
    private bool isDisposed;

    private readonly ProjectDbContext context;

    private readonly ProductService service;

    private readonly Mock<ILogger<ProductService>> loggerMock;

    private int testCategoryId;

    public ProductServiceTests()
    {
        var options = new DbContextOptionsBuilder<ProjectDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        this.context = new ProjectDbContext(options);

        SeedTestCategory();

        this.loggerMock = new Mock<ILogger<ProductService>>();

        this.service = new ProductService(this.context, this.loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnProduct()
    {
        // Arrange
        var expected = new Product
        {
            Name = "Test product for adding",
            CategoryId = testCategoryId,
            Price = 150
        };

        // Act
        var actual = await service.CreateAsync(expected);

        // Assert
        Assert.True(actual.Id > 0);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.CategoryId, actual.CategoryId);
        Assert.Equal(expected.Price, actual.Price);
    }

    [Fact]
    public async Task DeleteAsync_WhenFound_ShouldRemoveAndReturnTrue()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test product for deleting",
            CategoryId = testCategoryId,
            Price = 200
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();

        var expected = true;

        // Act
        var actual = await service.DeleteAsync(product.Id);

        // Assert
        Assert.Equal(expected, actual);
        Assert.Null(await context.Products.FindAsync(product.Id));
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ShouldReturnFalse()
    {
        // Arrange
        var expected = false;

        // Act
        var actual = await service.DeleteAsync(-1);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAll()
    {
        // Arrange
        Product[] products = new Product[]
        {
            new Product
            {
                Name = "Existing product 1",
                CategoryId = testCategoryId,
                Price = 349
            },
            new Product
            {
                Name = "Existing product 2",
                CategoryId = testCategoryId,
                Price = 3892
            }
        };

        context.Products.AddRange(products);

        await context.SaveChangesAsync();

        var expected = products.Length;

        // Act
        var result = await service.GetAllAsync();

        var actual = result.Count();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnProduct()
    {
        // Arrange
        var expected = new Product
        {
            Name = "Test product for getting product by id",
            CategoryId = testCategoryId,
            Price = 150
        };

        context.Products.Add(expected);

        await context.SaveChangesAsync();

        // Act
        var actual = await service.GetByIdAsync(expected.Id);

        // Assert
        Assert.NotNull(actual);
        Assert.True(actual.Id > 0);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.CategoryId, actual.CategoryId);
        Assert.Equal(expected.Price, actual.Price);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange

        // Act
        var result = await service.GetByIdAsync(-1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenExists_ShouldUpdateAndReturnProduct()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test product for updating",
            CategoryId = testCategoryId,
            Price = 200
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();

        var expected = new Product
        {
            Name = "Test product updated",
            CategoryId = testCategoryId,
            Price = 445
        };

        // Act
        var actual = await service.UpdateAsync(product.Id, expected);

        // Assert
        Assert.NotNull(actual);
        Assert.True(actual.Id > 0);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.CategoryId, actual.CategoryId);
        Assert.Equal(expected.Price, actual.Price);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test product for updating",
            CategoryId = testCategoryId,
            Price = 200
        };

        // Act
        var result = await service.UpdateAsync(-1, product);

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Helper method for seeding a test category required for product creation.
    /// </summary>
    private void SeedTestCategory()
    {
        var category = new Category { Name = "Test category" };

        this.context.Categories.Add(category);

        this.context.SaveChanges();

        this.testCategoryId = category.Id;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.isDisposed)
        {
            this.isDisposed = true;

            if (disposing)
            {
                context.Dispose();
            }
        }
    }
}
