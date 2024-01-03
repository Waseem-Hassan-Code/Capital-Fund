using Capital.Funds.Database;
using Capital.Funds.EndPoints;
using Capital.Funds.Models;
using Capital.Funds.Services;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<ApplicationDb>(options =>
{
    options.UseSqlite($"Data source={AppDomain.CurrentDomain.BaseDirectory}{SD.DatabaseName}");
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IManageTenants,ManageTenants>();
builder.Services.AddScoped<IBuildingManagment,BuildingMangment>();
builder.Services.AddScoped<ITenatsResidencyInfo, TenatsResidencyInfo>();
builder.Services.AddScoped<ITenantPayments, TenantPayment>();
builder.Services.AddScoped<ITenantsComplains, TenantsComplains>();
builder.Services.AddScoped<IUserEssentials,UserEssentials>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "Jwt",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


builder.AddAppAuthetication();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireClaim("Role", "admin");
    });

    options.AddPolicy("UserOnly", policy =>
    {
        policy.RequireClaim("Role", "user");
    });
});


var app = builder.Build();




if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.ConfigureAuthEndpoints();
app.ConfigureManageBuildingsEndPoints();
app.ConfigureManageEmployeeEndPoints();
app.ConfigureTenantsResidencyInfo();
app.ConfigureTenantsPaymentsInfo();
app.ConfigureUserEssentialsEndPoints();
app.UseHttpsRedirection();



app.Run();
