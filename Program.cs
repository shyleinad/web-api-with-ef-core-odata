using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using System;
using web_api_with_ef_core_odata.Data;
using web_api_with_ef_core_odata.Models;

var builder = WebApplication.CreateBuilder(args);

// OData model
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Product>("Products");
modelBuilder.EntitySet<Category>("Categories");

// Add services to the container.

// Add OData services
builder.Services.AddControllers()
    .AddOData(opt => opt
    .AddRouteComponents("odata", modelBuilder.GetEdmModel()).
    Select().
    Filter().
    OrderBy().
    SetMaxTop(100).
    Count().
    SkipToken().
    Expand());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Config db
builder.Services.AddDbContext<ProjectDbContext>(opt => opt.UseInMemoryDatabase("ProductsDb"));
// Logging
builder.Services.AddLogging(cfg => cfg.AddConsole());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply routing for OData
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// Initialize the database with seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
    await db.Database.EnsureCreatedAsync();
}

await app.RunAsync();
