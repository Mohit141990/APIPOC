using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using WebAPI5.Configuration;
using WebAPI5.Database;
using System.Text;
using WebAPI5.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using WebAPI5;
using Microsoft.AspNetCore.Mvc;
using WebAPI5.Middleware;
using AspNetCoreRateLimit;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddMemoryCache();
builder.Services.AddControllers();

// Load general and IP specific rate limit rules from configuration
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Configure rate limiting policies
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.Configure<RateLimitOptions>(builder.Configuration.GetSection("GeneralRules"));
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:5132"; // Redis connection string
    options.InstanceName = "SampleInstance"; // Optional
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new QueryStringApiVersionReader("v");
    //options.AssumeDefaultVersionWhenUnspecified = true;
    //options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    //options.ReportApiVersions = true;
    //options.ApiVersionReader = new UrlSegmentApiVersionReader();
    //options.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(opt =>
{
    //opt.OrderActionsBy((apiDesc) => $"{swaggerControllerOrder.SortKey(apiDesc.ActionDescriptor.RouteValues["controller"])}");

    opt.EnableAnnotations();
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My First Web API"
    });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAutoMapper(typeof(Program));

/*WebAPI : this is the example of Dependency Injection*/
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddSingleton<IReposotory, InMemory>();
builder.Services.AddScoped<IUserLogin, UserLogin>();
builder.Services.AddTransient<CustomeModelBuilder>();

builder.Services.AddScoped<IStudentRepo, StudentRepo>();

builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("Jwt"));

/*Add Identity application context */
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<DatabaseContext>()
        .AddDefaultTokenProviders();

//builder.Services.AddAuthentication(options => { options.DefaultScheme = null; });
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
/*Web API Note : Middleware example */
app.UseHttpsRedirection();

app.UseAuthorization();
/*TO DO :: Practical pending*/
/*Web API Note : this is the example of Conventional routing*/
//app.MapControllerRoute(
//    name: "Myget1",
//    pattern: "{controller=Country}/{action=GetData}");

//app.MapControllerRoute(
//    name: "Myget",
//    pattern: "{controller=Country}/{action=GetData}/{id?}");

//app.MapControllerRoute(
//    name: "MyPost",
//    pattern: "{controller=Country}/{action=PostData}/{country?}");



app.MapControllers();


/*Web API Note : this is the example of Multiple conventional routing*/
//app.MapControllerRoute(name: "blog",
//                pattern: "blog/{*article}",
//                defaults: new { controller = "Blog", action = "Article" });
//app.MapControllerRoute(name: "default",
//               pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseMiddleware<WebhookAuthenticationMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
app.Run();

