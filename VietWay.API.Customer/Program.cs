using IdGen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VietWay.API.Customer.Mappers;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Implement;
using VietWay.Service.Interface;
using VietWay.Service.ThirdParty;
using VietWay.Util.IdHelper;

namespace VietWay.API.Customer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Customer/GetCustomerInfo-specific services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ITourService, TourService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Shared services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            #region builder.Services.AddAuthentication(...);
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                string? issuer = builder.Configuration["Jwt:Issuer"]
                        ?? throw new Exception("Can not get JWT Issuer");
                string? audience = builder.Configuration["Jwt:Audience"]
                    ?? throw new Exception("Can not get JWT Audience");
                string secretKey;
                if (builder.Environment.IsDevelopment())
                {
                    secretKey = builder.Configuration["Jwt:Key"]
                        ?? throw new Exception("Can not get JWT Key");
                }
                else
                {
                    secretKey = Environment.GetEnvironmentVariable("PROD_JWT_KEY")
                        ?? throw new Exception("Can not get JWT Key");
                }
                o.UseSecurityTokenValidators = true;
                o.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateLifetime = true
                };
            });
            #endregion

            #region builder.Services.AddCors(...);
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            #endregion

            #region builder.Services.AddSwaggerGen(...);
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new OpenApiInfo
                { Title = "VietWay API", Description = "API for VietWay", Version = "1.0.0" });
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });
            #endregion
            #region builder.Services.AddScoped(...);
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IVnPayService, VnPayService>();
            builder.Services.AddScoped<ITourService, TourService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            #region builder.Services.AddScoped(...);
            // Add third-party services
            builder.Services.AddScoped<IVnPayService, VnPayService>();
            #endregion

            builder.Services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();

            var app = builder.Build();

            app.UseStaticFiles();

            #region app.UseSwagger(...);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.InjectStylesheet("/SwaggerDark.css");
                    c.EnableTryItOutByDefault();
                });
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VietWay Customer Api");
                    c.InjectStylesheet("/SwaggerDark.css");
                    c.RoutePrefix = "";
                    c.EnableTryItOutByDefault();
                });
            }
            #endregion

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<ErrorHandlingMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
}