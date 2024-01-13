using Capital.Funds.Database;
using Capital.Funds.EndPoints;
using Capital.Funds.Models;
using Capital.Funds.ScheduledJobs;
using Capital.Funds.Services;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quartz;

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
builder.Services.AddScoped<IDropDownLists, DropDownLists>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<FileHandling>();

builder.Services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();

    var jobKeyMonthlyPayments = JobKey.Create(nameof(TriggerPayments) + "_MonthlyPayments");
    options.AddJob<TriggerPayments>(jobKeyMonthlyPayments)
        .AddTrigger(trigger => trigger.ForJob(jobKeyMonthlyPayments)
            .WithCronSchedule("0 0 4 1 * ?"));

    var jobKeyLateFee = JobKey.Create(nameof(TriggerPayments) + "_LateFee");
    options.AddJob<TriggerPayments>(jobKeyLateFee)
        .AddTrigger(trigger => trigger.ForJob(jobKeyLateFee)
            .WithCronSchedule("0 0 11 1 * ?"));

    //var jobKeyTest = JobKey.Create(nameof(TriggerPayments) + "_Test");
    //options.AddJob<TriggerPayments>(jobKeyTest)
    //    .AddTrigger(trigger => trigger.ForJob(jobKeyTest)
    //        .WithCronSchedule("0/5 * * ? * *"));
});

builder.Services.AddQuartzHostedService();


//builder.Services.AddSingleton<DriveService>(provider =>
//{
//    string credentialsPath = SD.googleDriveAPIKey;
//    GoogleCredential credential;
//    using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
//    {
//        credential = GoogleCredential.FromStream(stream)
//            .CreateScoped(DriveService.ScopeConstants.Drive);
//    }
//    var service = new DriveService(new BaseClientService.Initializer()
//    {
//        HttpClientInitializer = credential,
//        ApplicationName = "Tenants Record",
//    });
//    return service;
//});

builder.Services.AddScoped<FileHandling>(); 


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
app.ConfigureComplainsEndPoints();
app.ConfigureDropDownsEndPoints();
app.UseHttpsRedirection();


app.Run();
