using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using VietWay.API.Customer.Mappers;
using VietWay.Middleware;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Implement;
using VietWay.Service.Interface;
using VietWay.Service.ThirdParty;
using VietWay.Util;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.HashUtil;
using VietWay.Util.IdUtil;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Customer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if (builder.Environment.IsDevelopment())
            {
                DotEnv.Load(".env");
            }
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ITourService, TourService>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddControllers();
            builder.Services.AddLogging();
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
                string issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                    ?? throw new Exception("JWT_ISSUER is not set in environment variables");
                string audience = Environment.GetEnvironmentVariable("JWT_ISSUER")
                    ?? throw new Exception("JWT_ISSUER is not set in environment variables");
                string secretKey = Environment.GetEnvironmentVariable("JWT_KEY")
                    ?? throw new Exception("JWT_KEY is not set in environment variables");
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
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.SwaggerDoc("v1",
                new OpenApiInfo
                { 
                    Title = "VietWay API", 
                    Description = "API for VietWay.<br/> {WIP} API endpoints has not been implemented yet", 
                    Version = "1.0.0" 
                });
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
            builder.Services.AddScoped<ITourTemplateService, TourTemplateService>();
            builder.Services.AddScoped<IProvinceService, ProvinceService>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
            builder.Services.AddScoped<ITourBookingService,TourBookingService>();
            builder.Services.AddScoped<IBookingPaymentService, BookingPaymentService>();
            builder.Services.AddScoped<ITimeZoneHelper, TimeZoneHelper>();
            builder.Services.AddScoped<IHashHelper, BCryptHashHelper>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITokenHelper, TokenHelper>();
            builder.Services.AddScoped<IHashHelper, BCryptHashHelper>();
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
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
}
