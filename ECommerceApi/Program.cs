using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using ECommerceApi.Interfaces;
using Backend_e_commerce_website.Interfaces;
using Backend_e_commerce_website.Services;
using ECommerceApi.Helpers;
using ECommerceApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MySQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySQLConnection"),
        Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection"))
    )
);

// Configure MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
    return new MongoClient(mongoConnectionString);
});
builder.Services.AddSingleton<MongoDbContext>();

// Auto register all AutoMapper PRofile
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Register Helper
builder.Services.AddTransient<PasswordHelper>();


// Register Service
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();



// Build the app
var app = builder.Build();

// Test MySQL connection
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        Console.WriteLine("Testing MySQL connection...");
        var canConnect = dbContext.Database.CanConnect();
        if (canConnect)
        {
            Console.WriteLine("MySQL connection successful!");
        }
        else
        {
            Console.WriteLine("Failed to connect to MySQL!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error testing MySQL connection: {ex.Message}");
    }
}

// Test MongoDB connection
try
{
    var mongoClient = app.Services.GetRequiredService<IMongoClient>();
    Console.WriteLine("Testing MongoDB connection...");
    var databaseList = mongoClient.ListDatabaseNames().ToList(); // Attempt to list databases
    Console.WriteLine("MongoDB connection successful!");
    Console.WriteLine($"Databases: {string.Join(", ", databaseList)}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error testing MongoDB connection: {ex.Message}");
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
