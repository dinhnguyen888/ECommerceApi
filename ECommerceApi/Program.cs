using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ECommerceApi.Interfaces;
using ECommerceApi.Helpers;
using ECommerceApi.Services;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using VNPAY.NET;

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
    options.CustomOperationIds(e => $"{e.HttpMethod}_{e.RelativePath}");
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
    if (string.IsNullOrEmpty(mongoConnectionString))
    {
        throw new Exception("MongoDB connection string is missing");
    }
    return new MongoClient(mongoConnectionString);
});
builder.Services.AddSingleton<MongoDbContext>();

// Auto register all AutoMapper Profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register Helpers and Services
builder.Services.AddSingleton<PasswordHelper>(); // Hoặc Transient nếu cần

builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IGitHubService, GitHubService>();
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<VnpayService>(); // Đổi từ Transient -> Scoped
builder.Services.AddTransient<IEmailService, EmailService>(); // Đổi từ Transient để tránh xung đột
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<MomoService>();

//builder.Services.AddSingleton(new MomoService(
//        builder.Configuration["MoMo:PartnerCode"],
//         builder.Configuration["MoMo:AccessKey"],
//        builder.Configuration["MoMo:SecretKey"],
//         builder.Configuration["MoMo:Endpoint"]
//    ));
//builder.Services.AddSingleton<IVnpay, Vnpay>();

//builder.Services.AddScoped<IUserProfileService, UserProfileService>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddHttpContextAccessor();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("JWT Key is missing in configuration");
}

var key = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = "Cookies";
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
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/api/Oauth/login"; // URL đăng nhập

})
.AddOAuth("GitHub", opt =>
{
    opt.ClientId = builder.Configuration["Github:ClientId"];
    opt.ClientSecret = builder.Configuration["Github:ClientSecret"];
    opt.CallbackPath = new PathString("/api/OAuth/github-login");
    opt.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
    opt.TokenEndpoint = "https://github.com/login/oauth/access_token";
    opt.UserInformationEndpoint = "https://api.github.com/user";
    opt.SaveTokens = true;

    opt.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
    opt.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
    opt.ClaimActions.MapJsonKey("urn:github:name", "name");
    opt.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
    opt.ClaimActions.MapJsonKey("urn:github:url", "html_url");
}

)
.AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = builder.Configuration["Facebook:AppId"];
    facebookOptions.AppSecret = builder.Configuration["Facebook:AppSecret"];
    facebookOptions.CallbackPath = "/callback/facebook";
    facebookOptions.SaveTokens = true;
    facebookOptions.Events.OnRedirectToAuthorizationEndpoint = (redirectContext) =>
    {
        Console.WriteLine(redirectContext.RedirectUri);
        redirectContext.Response.Redirect(redirectContext.RedirectUri);
        return Task.CompletedTask;
    };
    facebookOptions.Events.OnTicketReceived = (context) =>
    {
        Console.WriteLine(context.HttpContext.User);
        return Task.CompletedTask;
    };
    facebookOptions.Events.OnCreatingTicket = (context) =>
    {
        Console.WriteLine(context.Identity);
        return Task.CompletedTask;
    };
    facebookOptions.Events.OnAccessDenied = (context) =>
    {
        Console.WriteLine(context.HttpContext.User);
        return Task.CompletedTask;
    };

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});


// Build the app
//builder.WebHost.UseUrls("http://*:7202");
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services);
}

// Middleware setup
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
