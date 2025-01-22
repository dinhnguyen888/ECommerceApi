using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ECommerceApi.Interfaces;
using ECommerceApi.Helpers;
using ECommerceApi.Services;
using Backend_e_commerce_website.Interfaces;
using Backend_e_commerce_website.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "E-Commerce API",
        Version = "v1",
        Description = "API for E-Commerce Application"
    });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsIn...\""
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


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

// Auto register all AutoMapper Profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register Helper
builder.Services.AddTransient<PasswordHelper>();

// Register Service
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

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
    var databaseList = mongoClient.ListDatabaseNames().ToList();
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

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
