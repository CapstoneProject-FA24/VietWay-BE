using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using VietWay.API.Management.Mappers;
using VietWay.Repository.UnitOfWork;
using VietWay.Middleware;
using System.Reflection;
using VietWay.Util.IdUtil;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.TokenUtil;
using Hangfire;
using VietWay.Util;
using VietWay.Util.HashUtil;
using VietWay.Service.Management.Implement;
using VietWay.Service.Management.Interface;
using VietWay.Service.ThirdParty.Cloudinary;
using VietWay.Service.ThirdParty.VnPay;
using VietWay.Service.ThirdParty.Twitter;
namespace VietWay.API.Management
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
            #region builder.Services.AddHangfire(...);
            builder.Services.AddHangfire(option =>
            {
                string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") 
                    ?? throw new Exception("SQL_CONNECTION_STRING is not set in environment variables");
                option.UseSqlServerStorage(connectionString);
            });
            #endregion
            builder.Services.AddHangfireServer();
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
                string issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                    ?? throw new Exception("JWT_ISSUER is not set in environment variables");
                string audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                    ?? throw new Exception("JWT_AUDIENCE is not set in environment variables");
                string secretKey = Environment.GetEnvironmentVariable("JWT_KEY")
                    ?? throw new Exception("JWT_KEY is not set in environment variables");
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
                { Title = "VietWay Management API", Description = "Management API for VietWay", Version = "1.0.0" });
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
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAttractionService, AttractionService>();
            builder.Services.AddScoped<IAttractionTypeService , AttractionTypeService>();
            builder.Services.AddScoped<IBookingPaymentService, BookingPaymentService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<ICustomerFeedbackService, CustomerFeedbackService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IEventCategoryService, EventCategoryService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IManagerService, ManagerService>();
            builder.Services.AddScoped<IPostCategoryService, PostCategoryService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IProvinceService, ProvinceService>();
            builder.Services.AddScoped<IStaffService, StaffService>();
            builder.Services.AddScoped<ITourCategoryService, TourCategoryService>();
            builder.Services.AddScoped<ITourDurationService, TourDurationService>();
            builder.Services.AddScoped<ITourService, TourService>();
            builder.Services.AddScoped<ITourTemplateService, TourTemplateService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IVnPayService, VnPayService>();
            builder.Services.AddScoped<ITimeZoneHelper, TimeZoneHelper>();
            builder.Services.AddScoped<IHashHelper, BCryptHashHelper>();
            builder.Services.AddScoped<ITokenHelper, TokenHelper>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
            builder.Services.AddScoped<ITwitterService, TwitterService>();
            builder.Services.AddScoped<IPublishPostService, PublishPostService>();
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VietWay Api");
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
            app.UseHangfireDashboard("/hangfire");
            app.MapControllers();
            app.Run();
        }
    }
}
