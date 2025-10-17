using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using web_api_with_ef_core_odata.Data;
using web_api_with_ef_core_odata.Models;
using web_api_with_ef_core_odata.Services;

namespace web_api_with_ef_core_odata.Tests;

public class CategoryServiceTests : IDisposable
{
    private bool isDisposed;

    private readonly ProjectDbContext context;

    private readonly CategoryService service;

    private readonly Mock<ILogger<CategoryService>> loggerMock;

    public CategoryServiceTests()
    {
        var options = new DbContextOptionsBuilder<ProjectDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        this.context = new ProjectDbContext(options);


        this.loggerMock = new Mock<ILogger<CategoryService>>();

        this.service = new CategoryService(this.context, this.loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreate()
    {
        // Arrange
        var expected = new Category
        {
            Name = "Test category for adding",
        };

        // Act
        var actual = await service.CreateAsync(expected);

        // Assert
        Assert.True(actual.Id > 0);
        Assert.Equal(expected.Name, actual.Name);
    }

    [Fact]
    public async Task DeleteAsync_WhenFound_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        var category = new Category
        {
            Name = "Test category for deleting",
        };

        context.Categories.Add(category);

        await context.SaveChangesAsync();

        var expected = true;

        // Act
        var actual = await service.DeleteAsync(category.Id);

        // Assert
        Assert.Equal(expected, actual);
        Assert.Null(await context.Categories.FindAsync(category.Id));
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
        Category[] categories = new Category[]
        {
            new Category
            {
                Name = "Existing category 1",
            },
            new Category
            {
                Name = "Existing category 2",
            }
        };

        context.Categories.AddRange(categories);

        await context.SaveChangesAsync();

        var expected = categories.Length;

        // Act
        var result = await service.GetAllAsync();

        var actual = result.Count();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnCategory()
    {
        // Arrange
        var expected = new Category
        {
            Name = "Test category for getting category by id",
        };

        context.Categories.Add(expected);

        await context.SaveChangesAsync();

        // Act
        var actual = await service.GetByIdAsync(expected.Id);

        // Assert
        Assert.NotNull(actual);
        Assert.True(actual.Id > 0);
        Assert.Equal(expected.Name, actual.Name);
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
    public async Task UpdateAsync_WhenExists_ShouldUpdateAndReturnCategory()
    {
        // Arrange
        var category = new Category
        {
            Name = "Test category for updating",
        };

        context.Categories.Add(category);

        await context.SaveChangesAsync();

        var expected = new Category
        {
            Name = "Test category updated",
        };

        // Act
        var actual = await service.UpdateAsync(category.Id, expected);

        // Assert
        Assert.NotNull(actual);
        Assert.True(actual.Id > 0);
        Assert.Equal(expected.Name, actual.Name);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var category = new Category
        {
            Name = "Test category for updating",
        };

        // Act
        var result = await service.UpdateAsync(-1, category);

        // Assert
        Assert.Null(result);
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