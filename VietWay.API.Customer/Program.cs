using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;
using System.Text;
using VietWay.API.Customer.Mappers;
using VietWay.Middleware;
using VietWay.Repository.DataAccessObject;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.Configuration;
using VietWay.Service.Customer.Implementation;
using VietWay.Service.Customer.Interface;
using VietWay.Service.ThirdParty.Firebase;
using VietWay.Service.ThirdParty.GoogleGemini;
using VietWay.Service.ThirdParty.VnPay;
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

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FIREBASE_CREDENTIAL") ??
                    throw new Exception("FIREBASE_CREDENTIAL is not set in environment variables"))
            });

            #region builder.Services.AddHangfire(...);
            builder.Services.AddHangfire(option =>
            {
                string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
                    ?? throw new Exception("SQL_CONNECTION_STRING is not set in environment variables");
                option.UseSqlServerStorage(connectionString);
            });
            #endregion
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
                string audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                    ?? throw new Exception("JWT_ISSUER is not set in environment variables");
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
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Token validation failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    }
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
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAttractionCategoryService, AttractionCategoryService>();
            builder.Services.AddScoped<IAttractionService, AttractionService>();
            builder.Services.AddScoped<IAttractionReviewService, AttractionReviewService>();
            builder.Services.AddScoped<IBookingPaymentService, BookingPaymentService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IPostCategoryService, PostCategoryService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IProvinceService, ProvinceService>();
            builder.Services.AddScoped<ITourCategoryService, TourCategoryService>();
            builder.Services.AddScoped<ITourDurationService, TourDurationService>();
            builder.Services.AddScoped<ITourService, TourService>();
            builder.Services.AddScoped<ITourTemplateService, TourTemplateService>();
            builder.Services.AddScoped<IVnPayService, VnPayService>();
            builder.Services.AddScoped<ITourReviewService, TourReviewService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ITimeZoneHelper, TimeZoneHelper>();
            builder.Services.AddScoped<IHashHelper, BCryptHashHelper>();
            builder.Services.AddScoped<ITokenHelper, TokenHelper>();
            builder.Services.AddScoped<IFirebaseService, FirebaseService>();
            #endregion
            builder.Services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
                .Connect(Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ??
                    throw new Exception("REDIS_CONNECTION_STRING is not set in environment variables")));
            builder.Services.AddSingleton(s => new GeminiApiConfig
            {
                ApiKey = Environment.GetEnvironmentVariable("GEMINI_AI_API_KEY") ??
                    throw new Exception("GEMINI_AI_API_KEY is not set in environment variables"),
                SystemPrompt = Environment.GetEnvironmentVariable("GEMINI_AI_SYSTEM_PROMPT")
            });
            builder.Services.AddSingleton(s => new DatabaseConfig
            {
                ConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") ??
                    throw new Exception("SQL_CONNECTION_STRING is not set in environment variables")
            });
            builder.Services.AddSingleton(s => new TokenConfig
            {
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ??
                    throw new Exception("JWT_AUDIENCE is not set in environment variables"),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ??
                    throw new Exception("JWT_ISSUER is not set in environment variables"),
                Secret = Environment.GetEnvironmentVariable("JWT_KEY") ??
                    throw new Exception("JWT_KEY is not set in environment variables")
            });
            builder.Services.AddSingleton(s => new BookingServiceConfiguration
            {
                PendingBookingExpireAfterMinutes = int.Parse(Environment.GetEnvironmentVariable("PENDING_BOOKING_EXPIRE_AFTER_MINUTES") ??
                    throw new Exception("PENDING_BOOKING_EXPIRE_AFTER_MINUTES is not set in environment variables"))
            });
            builder.Services.AddSingleton(s => new TourReviewServiceConfiguration
            {
                ReviewTourExpireAfterDays = int.Parse(Environment.GetEnvironmentVariable("REVIEW_TOUR_EXPIRE_AFTER_DAYS") ??
                    throw new Exception("REVIEW_TOUR_EXPIRE_AFTER_DAYS is not set in environment variables"))
            });
            builder.Services.AddSingleton(s => new VnPayConfiguration
            {
                VnpHashSecret = Environment.GetEnvironmentVariable("VNP_HASH_SECRET") ??
                    throw new Exception("VNP_HASH_SECRET is not set in environment variables"),
                VnpTmnCode = Environment.GetEnvironmentVariable("VNP_TMN_CODE") ??
                    throw new Exception("VNP_TMN_CODE is not set in environment variables")
            });
            builder.Services.AddHttpClient<IGeminiService,GeminiService>(HttpClient =>
            {
                string baseUrl = Environment.GetEnvironmentVariable("GEMINI_AI_API_ENDPOINT") ??
                    throw new Exception("GEMINI_AI_API_ENDPOINT is not set in environment variables");
                HttpClient.BaseAddress = new Uri(baseUrl);
            });
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
