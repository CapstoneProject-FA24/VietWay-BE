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
using VietWay.Service.ThirdParty.Redis;
using VietWay.Service.ThirdParty.Sms;
using VietWay.Service.ThirdParty.VnPay;
using VietWay.Util;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.HashUtil;
using VietWay.Util.IdUtil;
using VietWay.Util.OtpUtil;
using VietWay.Util.TokenUtil;
using VietWay.Service.ThirdParty.Email;
using VietWay.Service.ThirdParty.ZaloPay;
using VietWay.Service.ThirdParty.PayOS;

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
            builder.Services.AddScoped<IOtpGenerator, OtpGenerator>();
            builder.Services.AddScoped<ISmsService, SmsService>();
            builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
            builder.Services.AddScoped<IZaloPayService, ZaloPayService>();
            builder.Services.AddScoped<IPayOSService, PayOSService>();
            #endregion
            #region builder.Services.AddSingleton(...);
            builder.Services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
                .Connect(Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ??
                    throw new Exception("REDIS_CONNECTION_STRING is not set in environment variables")));
            builder.Services.AddSingleton(s => new GeminiApiConfig(
                apiKey: Environment.GetEnvironmentVariable("GEMINI_AI_API_KEY") ??
                    throw new Exception("GEMINI_AI_API_KEY is not set in environment variables"),
                systemPrompt: Environment.GetEnvironmentVariable("GEMINI_AI_SYSTEM_PROMPT"),
                infoExtractSystemPrompt: Environment.GetEnvironmentVariable("GEMINI_AI_SYSTEM_PROMPT_EXTRACTION")
            ));
            builder.Services.AddSingleton(s => new DatabaseConfig(
                connectionString: Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") ??
                    throw new Exception("SQL_CONNECTION_STRING is not set in environment variables")
            ));
            builder.Services.AddSingleton(s => new TokenConfig(
                issuer: Environment.GetEnvironmentVariable("JWT_ISSUER") ??
                    throw new Exception("JWT_ISSUER is not set in environment variables"),
                audience: Environment.GetEnvironmentVariable("JWT_AUDIENCE") ??
                    throw new Exception("JWT_AUDIENCE is not set in environment variables"),
                secret: Environment.GetEnvironmentVariable("JWT_KEY") ??
                    throw new Exception("JWT_KEY is not set in environment variables"),
                tokenExpireAfterMinutes: int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE_AFTER_MINUTES") ??
                    throw new Exception("JWT_EXPIRE_AFTER_MINUTES is not set in environment variables"))
            ));
            builder.Services.AddSingleton(s => new BookingServiceConfiguration(
                pendingBookingExpireAfterMinutes: int.Parse(Environment.GetEnvironmentVariable("PENDING_BOOKING_EXPIRE_AFTER_MINUTES") ??
                    throw new Exception("PENDING_BOOKING_EXPIRE_AFTER_MINUTES is not set in environment variables"))
            ));
            builder.Services.AddSingleton(s => new TourReviewServiceConfiguration(
                reviewTourExpireAfterDays: int.Parse(Environment.GetEnvironmentVariable("REVIEW_TOUR_EXPIRE_AFTER_DAYS") ??
                    throw new Exception("REVIEW_TOUR_EXPIRE_AFTER_DAYS is not set in environment variables"))
            ));
            builder.Services.AddSingleton(s => new VnPayConfiguration(
                vnpHashSecret: Environment.GetEnvironmentVariable("VNPAY_HASH_SECRET") ??
                    throw new Exception("VNPAY_HASH_SECRET is not set in environment variables"),
                vnpTmnCode: Environment.GetEnvironmentVariable("VNPAY_TMN_CODE") ??
                    throw new Exception("VNPAY_TMN_CODE is not set in environment variables"),
                returnUrl: Environment.GetEnvironmentVariable("RETURN_URL") ??
                    throw new Exception("RETURN_URL is not set in environment variables")
            ));
            builder.Services.AddSingleton(s => new OtpGeneratorConfiguration(
                length: int.Parse(Environment.GetEnvironmentVariable("SMS_OTP_LENGTH") ??
                    throw new Exception("SMS_OTP_LENGTH is not set in environment variables")),
                expiryTimeInMinute: int.Parse(Environment.GetEnvironmentVariable("SMS_OTP_EXPIRE_AFTER_MINUTES") ??
                    throw new Exception("SMS_OTP_EXPIRE_AFTER_MINUTES is not set in environment variables"))
            ));
            builder.Services.AddSingleton(s => new SmsConfiguration(
                deviceId: Environment.GetEnvironmentVariable("SPEEDSMS_DEVICE_ID") ??
                    throw new Exception("SPEEDSMS_DEVICE_ID is not set in environment variables"),
                sendTokenMessage: Environment.GetEnvironmentVariable("SMS_SEND_TOKEN_MESSAGE") ??
                    throw new Exception("SMS_SEND_TOKEN_MESSAGE is not set in environment variables"),
                token: Environment.GetEnvironmentVariable("SPEEDSMS_TOKEN") ??
                    throw new Exception("SPEEDSMS_TOKEN is not set in environment variables")
            ));
            builder.Services.AddSingleton(s => new ZaloPayServiceConfiguration(
                zaloPayAppId: Environment.GetEnvironmentVariable("ZALOPAY_APP_ID") ??
                    throw new Exception("ZALOPAY_APP_ID is not set in environment variables"),
                zaloPayAppUser: Environment.GetEnvironmentVariable("ZALOPAY_APP_USER") ??
                    throw new Exception("ZALOPAY_APP_USER is not set in environment variables"),
                zaloPayKey1: Environment.GetEnvironmentVariable("ZALOPAY_KEY1") ??
                    throw new Exception("ZALOPAY_KEY1 is not set in environment variables"),
                zaloPayKey2: Environment.GetEnvironmentVariable("ZALOPAY_KEY2") ??
                    throw new Exception("ZALOPAY_KEY2 is not set in environment variables"),
                returnUrl: Environment.GetEnvironmentVariable("RETURN_URL") ??
                    throw new Exception("RETURN_URL is not set in environment variables")
            ));
            builder.Services.AddSingleton(s => new PayOSConfiguration(
                apiKey: Environment.GetEnvironmentVariable("PAYOS_API_KEY") ??
                    throw new Exception("PAYOS_API_KEY is not set in environment variables"),
                checksumKey: Environment.GetEnvironmentVariable("PAYOS_CHECKSUM_KEY") ??
                    throw new Exception("PAYOS_CHECKSUM_KEY is not set in environment variables"),
                clientId: Environment.GetEnvironmentVariable("PAYOS_CLIENT_ID") ??
                    throw new Exception("PAYOS_CLIENT_ID is not set in environment variables"),
                returnUrl: Environment.GetEnvironmentVariable("RETURN_URL") ??
                    throw new Exception("RETURN_URL is not set in environment variables")
            ));
            builder.Services.AddSingleton(s => new BookingPaymentConfiguration(
                paymentExpireAfterMinutes: int.Parse(Environment.GetEnvironmentVariable("BOOKING_PAYMENT_EXPIRE_AFTER_MINUTES") ??
                    throw new Exception("BOOKING_PAYMENT_EXPIRE_AFTER_MINUTES is not set in environment variables"))
            ));
            #endregion
            builder.Services.AddHttpClient<IGeminiService, GeminiService>(HttpClient =>
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