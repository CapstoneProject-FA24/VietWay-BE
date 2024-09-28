
using IdGen.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VietWayManagementAPI.Mappers;

namespace VietWayManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddIdGen(0);
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                string issuer;
                string audience;
                string secretKey;
                if (builder.Environment.IsDevelopment())
                {
                    issuer = builder.Configuration["Jwt:Issuer"];
                    audience = builder.Configuration["Jwt:Audience"];
                    secretKey = builder.Configuration["Jwt:SecretKey"];
                } else
                {
                    issuer = Environment.GetEnvironmentVariable("Jwt:Issuer");
                    audience = Environment.GetEnvironmentVariable("Jwt:Audience");
                    secretKey = Environment.GetEnvironmentVariable("Jwt:SecretKey");
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
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
