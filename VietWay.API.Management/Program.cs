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
using VietWay.Service.ThirdParty.Facebook;
using VietWay.Service.ThirdParty.Redis;
using StackExchange.Redis;
using VietWay.Repository.DataAccessObject;
using VietWay.Job.Implementation;
using VietWay.Job.Interface;
using VietWay.Job.Configuration;
using VietWay.Service.ThirdParty.Email;
using VietWay.Service.ThirdParty.ZaloPay;
using Net.payOS.Constants;
using VietWay.Service.ThirdParty.PayOS;
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
            builder.Services.AddScoped<IAttractionTypeService, AttractionTypeService>();
            builder.Services.AddScoped<IBookingPaymentService, BookingPaymentService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<ICustomerFeedbackService, CustomerFeedbackService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IManagerService, ManagerService>();
            builder.Services.AddScoped<IPostCategoryService, PostCategoryService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IProvinceService, ProvinceService>();
            builder.Services.AddScoped<IStaffService, StaffService>();
            builder.Services.AddScoped<ITourCategoryService, TourCategoryService>();
            builder.Services.AddScoped<ITourDurationService, TourDurationService>();
            builder.Services.AddScoped<ITourService, TourService>();
            builder.Services.AddScoped<ITourTemplateService, TourTemplateService>();
            builder.Services.AddScoped<ITourReviewService, TourReviewService>();
            builder.Services.AddScoped<IAttractionReviewService, AttractionReviewService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IPublishPostService, PublishPostService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IEntityHistoryService, EntityHistoryService>();
            builder.Services.AddScoped<IBookingRefundService, BookingRefundService>();

            builder.Services.AddScoped<IVnPayService, VnPayService>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
            builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
            builder.Services.AddScoped<ITwitterService, TwitterService>();
            builder.Services.AddScoped<IEmailService, GmailService>();
            builder.Services.AddScoped<IZaloPayService, ZaloPayService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ITimeZoneHelper, TimeZoneHelper>();
            builder.Services.AddScoped<IHashHelper, BCryptHashHelper>();
            builder.Services.AddScoped<ITokenHelper, TokenHelper>();

            builder.Services.AddScoped<IEmailJob, EmailJob>();
            builder.Services.AddScoped<IProvinceJob, ProvinceJob>();
            builder.Services.AddScoped<ITourCategoryJob, TourCategoryJob>();
            builder.Services.AddScoped<ITourDurationJob, TourDurationJob>();
            builder.Services.AddScoped<IBookingJob, BookingJob>();
            builder.Services.AddScoped<ITourJob, TourJob>();
            builder.Services.AddScoped<ITweetJob, TweetJob>();

            #endregion
            builder.Services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();
            builder.Services.AddSingleton<IRecurringJobManager, RecurringJobManager>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
                .Connect(Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ??
                    throw new Exception("REDIS_CONNECTION_STRING is not set in environment variables")));
            builder.Services.AddSingleton(config => new FacebookApiConfig
            {
                PageId = Environment.GetEnvironmentVariable("FACEBOOK_PAGE_ID") ??
                    throw new Exception("FACEBOOK_PAGE_ID is not set in environment variables"),
                PageAccessToken = Environment.GetEnvironmentVariable("FACEBOOK_PAGE_ACCESS_TOKEN") ??
                    throw new Exception("FACEBOOK_PAGE_ACCESS_TOKEN is not set in environment variables")
            });
            builder.Services.AddSingleton(s => new CloudinaryApiConfig
            {
                ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") ??
                    throw new Exception("CLOUDINARY_API_KEY is not set in environment variables"),
                ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") ??
                    throw new Exception("CLOUDINARY_API_SECRET is not set in environment variables"),
                CloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") ??
                    throw new Exception("CLOUDINARY_CLOUD_NAME is not set in environment variables")
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
            builder.Services.AddSingleton(s => new TwitterServiceConfiguration
            {
                XApiKey = Environment.GetEnvironmentVariable("X_API_KEY") ??
                    throw new Exception("X_API_KEY is not set in environment variables"),
                XApiKeySecret = Environment.GetEnvironmentVariable("X_API_KEY_SECRET") ??
                    throw new Exception("X_API_KEY_SECRET is not set in environment variables"),
                XAccessToken = Environment.GetEnvironmentVariable("X_ACCESS_TOKEN") ??
                    throw new Exception("X_ACCESS_TOKEN is not set in environment variables"),
                XAccessTokenSecret = Environment.GetEnvironmentVariable("X_ACCESS_TOKEN_SECRET") ??
                    throw new Exception("X_ACCESS_TOKEN_SECRET is not set in environment variables"),
                BearerToken = Environment.GetEnvironmentVariable("X_BEARER_TOKEN") ??
                    throw new Exception("X_BEARER_TOKEN is not set in environment variables")
            });
            builder.Services.AddSingleton(s => new VnPayConfiguration
            {
                VnpHashSecret = Environment.GetEnvironmentVariable("VNPAY_HASH_SECRET") ??
                    throw new Exception("VNPAY_HASH_SECRET is not set in environment variables"),
                VnpTmnCode = Environment.GetEnvironmentVariable("VNPAY_TMN_CODE") ??
                    throw new Exception("VNPAY_TMN_CODE is not set in environment variables"),
                ReturnUrl = Environment.GetEnvironmentVariable("VNPAY_RETURN_URL") ??
                    throw new Exception("VNPAY_RETURN_URL is not set in environment variables")
            });
            builder.Services.AddSingleton(s => new EmailJobConfiguration
            {
                CancelBookingTemplate = Environment.GetEnvironmentVariable("MAIL_SEND_CANCEL_TOUR_MESSAGE") ??
                    throw new Exception("MAIL_SEND_CANCEL_TOUR_MESSAGE is not set in environment variables"),
                ConfirmBookingTemplate = Environment.GetEnvironmentVariable("MAIL_SEND_CONFIRM_TOUR_MESSAGE") ??
                    throw new Exception("MAIL_SEND_CONFIRM_TOUR_MESSAGE is not set in environment variables"),
            });
            builder.Services.AddSingleton(s => new EmailClientConfig
            {
                AppPassword = Environment.GetEnvironmentVariable("GOOGLE_APP_PASSWORD") ??
                    throw new Exception("GOOGLE_APP_PASSWORD is not set in environment variables"),
                SenderEmail = Environment.GetEnvironmentVariable("GOOGLE_SENDER_EMAIL") ??
                    throw new Exception("GOOGLE_SENDER_EMAIL is not set in environment variables"),
                SmtpHost = Environment.GetEnvironmentVariable("GOOGLE_SMTP_HOST") ??
                    throw new Exception("GOOGLE_SMTP_HOST is not set in environment variables"),
                SmtpPort = int.Parse(Environment.GetEnvironmentVariable("GOOGLE_SMTP_PORT") ??
                    throw new Exception("GOOGLE_SMTP_PORT is not set in environment variables")),
            });
            builder.Services.AddHttpClient<IFacebookService, FacebookService>(HttpClient =>
            {

                string graphApiBaseUrl = Environment.GetEnvironmentVariable("FACEBOOK_GRAPH_API_BASE_URL") ??
                    throw new Exception("FACEBOOK_GRAPH_API_BASE_URL is not set in environment variables");
                HttpClient.BaseAddress = new Uri(graphApiBaseUrl);
            });
            builder.Services.AddSingleton(s => new ZaloPayServiceConfiguration
            {
                ZaloPayAppId = Environment.GetEnvironmentVariable("ZALOPAY_APP_ID") ??
                    throw new Exception("ZALOPAY_APP_ID is not set in environment variables"),
                ZaloPayAppUser = Environment.GetEnvironmentVariable("ZALOPAY_APP_USER") ??
                    throw new Exception("ZALOPAY_APP_USER is not set in environment variables"),
                ZaloPayKey1 = Environment.GetEnvironmentVariable("ZALOPAY_KEY1") ??
                    throw new Exception("ZALOPAY_KEY1 is not set in environment variables"),
                ZaloPayKey2 = Environment.GetEnvironmentVariable("ZALOPAY_KEY2") ??
                    throw new Exception("ZALOPAY_KEY2 is not set in environment variables")
            });
            builder.Services.AddSingleton(s => new PayOSConfiguration
            {
                ApiKey = Environment.GetEnvironmentVariable("PAYOS_API_KEY") ??
                    throw new Exception("PAYOS_API_KEY is not set in environment variables"),
                ChecksumKey = Environment.GetEnvironmentVariable("PAYOS_CHECKSUM_KEY") ??
                    throw new Exception("PAYOS_CHECKSUM_KEY is not set in environment variables"),
                ClientId = Environment.GetEnvironmentVariable("PAYOS_CLIENT_ID") ??
                    throw new Exception("PAYOS_CLIENT_ID is not set in environment variables"),
                ReturnUrl = Environment.GetEnvironmentVariable("PAYOS_RETURN_URL") ??
                    throw new Exception("PAYOS_RETURN_URL is not set in environment variables"),
            });
            var app = builder.Build();

            IRecurringJobManager recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
            recurringJobManager
                .AddOrUpdate<ITweetJob>("getTweetsDetail", (x) => x.GetPublishedTweetsJob(), () => "*/16 * * * *");
            recurringJobManager
                .AddOrUpdate<IProvinceJob>("cacheProvinces", (x) => x.CacheProvinceJob(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ITourCategoryJob>("cacheTourCategories", (x) => x.CacheTourCategoryJob(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ITourDurationJob>("cacheTourDurations", (x) => x.CacheTourDurationJob(), () => "0 17 * * *");

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