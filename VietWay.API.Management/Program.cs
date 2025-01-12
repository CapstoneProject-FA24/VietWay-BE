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
using HangfireBasicAuthenticationFilter;
using Hangfire.Dashboard;
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
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IPopularService, PopularService>();

            builder.Services.AddScoped<IVnPayService, VnPayService>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
            builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
            builder.Services.AddScoped<ITwitterService, TwitterService>();
            builder.Services.AddScoped<IEmailService, GmailService>();
            builder.Services.AddScoped<IZaloPayService, ZaloPayService>();
            builder.Services.AddScoped<IPayOSService, PayOSService>();

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
            builder.Services.AddScoped<IBookingPaymentJob, BookingPaymentJob>();
            #endregion
            #region builder.Services.AddSingleton(...);
            builder.Services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();
            builder.Services.AddSingleton<IRecurringJobManager, RecurringJobManager>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
                .Connect(Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ??
                    throw new Exception("REDIS_CONNECTION_STRING is not set in environment variables")));
            builder.Services.AddSingleton(s => new FacebookApiConfig(
                pageId: Environment.GetEnvironmentVariable("FACEBOOK_PAGE_ID") ?? throw new Exception("FACEBOOK_PAGE_ID is not set in environment variables"),
                pageAccessToken: Environment.GetEnvironmentVariable("FACEBOOK_PAGE_ACCESS_TOKEN") ?? throw new Exception("FACEBOOK_PAGE_ACCESS_TOKEN is not set in environment variables")
));

            builder.Services.AddSingleton(s => new CloudinaryApiConfig(
                apiKey: Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") ?? throw new Exception("CLOUDINARY_API_KEY is not set in environment variables"),
                apiSecret: Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") ?? throw new Exception("CLOUDINARY_API_SECRET is not set in environment variables"),
                cloudName: Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") ?? throw new Exception("CLOUDINARY_CLOUD_NAME is not set in environment variables")
            ));

            builder.Services.AddSingleton(s => new DatabaseConfig(
                connectionString: Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") ?? throw new Exception("SQL_CONNECTION_STRING is not set in environment variables")
            ));

            builder.Services.AddSingleton(s => new TokenConfig(
                audience: Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new Exception("JWT_AUDIENCE is not set in environment variables"),
                issuer: Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new Exception("JWT_ISSUER is not set in environment variables"),
                secret: Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new Exception("JWT_KEY is not set in environment variables"),
                tokenExpireAfterMinutes: int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE_AFTER_MINUTES") ?? throw new Exception("JWT_EXPIRE_AFTER_MINUTES is not set in environment variables"))
            ));

            builder.Services.AddSingleton(s => new TwitterServiceConfiguration(
                xApiKey: Environment.GetEnvironmentVariable("X_API_KEY") ?? throw new Exception("X_API_KEY is not set in environment variables"),
                xApiKeySecret: Environment.GetEnvironmentVariable("X_API_KEY_SECRET") ?? throw new Exception("X_API_KEY_SECRET is not set in environment variables"),
                xAccessToken: Environment.GetEnvironmentVariable("X_ACCESS_TOKEN") ?? throw new Exception("X_ACCESS_TOKEN is not set in environment variables"),
                xAccessTokenSecret: Environment.GetEnvironmentVariable("X_ACCESS_TOKEN_SECRET") ?? throw new Exception("X_ACCESS_TOKEN_SECRET is not set in environment variables"),
                bearerToken: Environment.GetEnvironmentVariable("X_BEARER_TOKEN") ?? throw new Exception("X_BEARER_TOKEN is not set in environment variables")
            ));

            builder.Services.AddSingleton(s => new VnPayConfiguration(
                vnpHashSecret: Environment.GetEnvironmentVariable("VNPAY_HASH_SECRET") ?? throw new Exception("VNPAY_HASH_SECRET is not set in environment variables"),
                vnpTmnCode: Environment.GetEnvironmentVariable("VNPAY_TMN_CODE") ?? throw new Exception("VNPAY_TMN_CODE is not set in environment variables"),
                returnUrl: Environment.GetEnvironmentVariable("RETURN_URL") ?? throw new Exception("RETURN_URL is not set in environment variables")
            ));

            builder.Services.AddSingleton(s => new EmailJobConfiguration(
                cancelBookingTemplate: Environment.GetEnvironmentVariable("MAIL_SEND_CANCEL_TOUR_MESSAGE") ?? throw new Exception("MAIL_SEND_CANCEL_TOUR_MESSAGE is not set in environment variables"),
                confirmBookingTemplate: Environment.GetEnvironmentVariable("MAIL_SEND_CONFIRM_TOUR_MESSAGE") ?? throw new Exception("MAIL_SEND_CONFIRM_TOUR_MESSAGE is not set in environment variables"),
                vietwayCancelBookingTemplate: Environment.GetEnvironmentVariable("MAIL_SEND_VIETWAY_CANCEL_TOUR_MESSAGE") ?? throw new Exception("MAIL_SEND_VIETWAY_CANCEL_TOUR_MESSAGE is not set in environment variables"),
                resetPasswordTemplate: Environment.GetEnvironmentVariable("MAIL_SEND_RESET_PASSWORD_MESSAGE") ?? throw new Exception("MAIL_SEND_RESET_PASSWORD_MESSAGE is not set in environment variables"),
                bookingExpiredTemplate: Environment.GetEnvironmentVariable("MAIL_SEND_BOOKING_EXPIRED_MESSAGE") ?? throw new Exception("MAIL_SEND_BOOKING_EXPIRED_MESSAGE is not set in environment variables"),
                bookingChangedTemplate: Environment.GetEnvironmentVariable("MAIL_SEND_BOOKING_CHANGED_MESSAGE") ?? throw new Exception("MAIL_SEND_BOOKING_CHANGED_MESSAGE is not set in environment variables"),
                warningClosedTourNotEnoughParticipant: Environment.GetEnvironmentVariable("MAIL_SEND_WARNING_CLOSED_TOUR_NOT_ENOUGH_PARTICIPANT") ?? throw new Exception("MAIL_SEND_WARNING_CLOSED_TOUR_NOT_ENOUGH_PARTICIPANT is not set in environment variables")
            ));

            builder.Services.AddSingleton(s => new EmailClientConfig(
                appPassword: Environment.GetEnvironmentVariable("GOOGLE_APP_PASSWORD") ?? throw new Exception("GOOGLE_APP_PASSWORD is not set in environment variables"),
                senderEmail: Environment.GetEnvironmentVariable("GOOGLE_SENDER_EMAIL") ?? throw new Exception("GOOGLE_SENDER_EMAIL is not set in environment variables"),
                smtpHost: Environment.GetEnvironmentVariable("GOOGLE_SMTP_HOST") ?? throw new Exception("GOOGLE_SMTP_HOST is not set in environment variables"),
                smtpPort: int.Parse(Environment.GetEnvironmentVariable("GOOGLE_SMTP_PORT") ?? throw new Exception("GOOGLE_SMTP_PORT is not set in environment variables"))
            ));

            builder.Services.AddSingleton(s => new ZaloPayServiceConfiguration(
                zaloPayAppId: Environment.GetEnvironmentVariable("ZALOPAY_APP_ID") ?? throw new Exception("ZALOPAY_APP_ID is not set in environment variables"),
                zaloPayAppUser: Environment.GetEnvironmentVariable("ZALOPAY_APP_USER") ?? throw new Exception("ZALOPAY_APP_USER is not set in environment variables"),
                zaloPayKey1: Environment.GetEnvironmentVariable("ZALOPAY_KEY1") ?? throw new Exception("ZALOPAY_KEY1 is not set in environment variables"),
                zaloPayKey2: Environment.GetEnvironmentVariable("ZALOPAY_KEY2") ?? throw new Exception("ZALOPAY_KEY2 is not set in environment variables"),
                returnUrl: Environment.GetEnvironmentVariable("RETURN_URL") ?? throw new Exception("RETURN_URL is not set in environment variables")
            ));

            builder.Services.AddSingleton(s => new PayOSConfiguration(
                apiKey: Environment.GetEnvironmentVariable("PAYOS_API_KEY") ?? throw new Exception("PAYOS_API_KEY is not set in environment variables"),
                checksumKey: Environment.GetEnvironmentVariable("PAYOS_CHECKSUM_KEY") ?? throw new Exception("PAYOS_CHECKSUM_KEY is not set in environment variables"),
                clientId: Environment.GetEnvironmentVariable("PAYOS_CLIENT_ID") ?? throw new Exception("PAYOS_CLIENT_ID is not set in environment variables"),
                returnUrl: Environment.GetEnvironmentVariable("RETURN_URL") ?? throw new Exception("RETURN_URL is not set in environment variables")
            ));
            #endregion

            builder.Services.AddHttpClient<IFacebookService, FacebookService>(HttpClient =>
            {

                string graphApiBaseUrl = Environment.GetEnvironmentVariable("FACEBOOK_GRAPH_API_BASE_URL") ??
                    throw new Exception("FACEBOOK_GRAPH_API_BASE_URL is not set in environment variables");
                HttpClient.BaseAddress = new Uri(graphApiBaseUrl);
            });
            var app = builder.Build();

            IRecurringJobManager recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
            recurringJobManager
                .AddOrUpdate<ITweetJob>("getTweetsDetail", (x) => x.GetPublishedTweetsJob(), () => "*/20 * * * *");
            recurringJobManager
                .AddOrUpdate<ITweetJob>("getPopularHashtag", (x) => x.GetPopularHashtagJob(), () => "*/16 * * * *");
            recurringJobManager
                .AddOrUpdate<IProvinceJob>("cacheProvinces", (x) => x.CacheProvinceJob(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ITourCategoryJob>("cacheTourCategories", (x) => x.CacheTourCategoryJob(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ITourDurationJob>("cacheTourDurations", (x) => x.CacheTourDurationJob(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ITourJob>("openTours", (x) => x.OpenToursAsync(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ITourJob>("closeTours", (x) => x.CloseToursAsync(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ITourJob>("changeToursOngoing", (x) => x.ChangeToursToOngoingAsync(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ITourJob>("changeToursCompleted", (x) => x.ChangeToursToCompletedAsync(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ITourJob>("rejectClosedPendingTours", (x) => x.RejectUnapprovedToursAsync(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ReportJob>("generateMetrics", (x) => x.GetMetric(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ITweetJob>("getTweetsDetail", (x) => x.GetPublishedTweetsJob(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<FacebookJob>("getFacebookDetail", (x) => x.GetPublishedFacebookPostsJob(), () => "0 17 * * *");
            recurringJobManager
                .AddOrUpdate<ReportJob>("generateReport", (x) => x.GenerateReport(), () => "0 17 * * *");
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
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter  {
                        User = Environment.GetEnvironmentVariable("HANGFIRE_DASHBOARD_USERNAME") ??
                            throw new Exception("HANGFIRE_DASHBOARD_USERNAME is not set in environment variables"),
                        Pass = Environment.GetEnvironmentVariable("HANGFIRE_DASHBOARD_PASSWORD") ??
                            throw new Exception("HANGFIRE_DASHBOARD_PASSWORD is not set in environment variables")
                    }
                ]
            });
            app.MapControllers();
            app.Run();
        }
    }
}