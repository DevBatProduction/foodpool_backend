using System.Text;
using FoodPool.auth;
using FoodPool.auth.interfaces;
using FoodPool.data;
using FoodPool.order;
using FoodPool.order.interfaces;
using FoodPool.post;
using FoodPool.post.interfaces;
using FoodPool.provider;
using FoodPool.provider.interfaces;
using FoodPool.stall;
using FoodPool.stall.interfaces;
using FoodPool.user;
using FoodPool.user.interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Map Repository with Interface
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IStallRepository, StallRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IPostRepository, PostRepository>();

//Map Service with Interface
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStallService, StallService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IHttpContextProvider, HttpContextProvider>();

//Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//Contect PostgreSQL
builder.Services.AddDbContext<FoodpoolDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
});

//Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["secretKey"]!))
    };
});

//Context aka req.user
builder.Services.AddHttpContextAccessor();

// Add Authorization
builder.Services.AddAuthorization();

//Cors
const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "https://foodpool.bxdman.com", "https://script.google.com")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();