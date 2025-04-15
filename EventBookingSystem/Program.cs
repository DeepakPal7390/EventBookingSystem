
//using EventBookingSystem.Data;
//using EventBookingSystem.Repositories.Implementation;
//using EventBookingSystem.Repositories.Interfaces;
//using EventBookingSystem.Services.Implementation;
//using EventBookingSystem.Services.Interfaces;
//using Microsoft.EntityFrameworkCore;

//// Start impliment Jwt authentication
//using EventBookingSystem.Middleware;
//using Microsoft.AspNetCore.Authentication.JwtBearer;


//using Microsoft.IdentityModel.Tokens;
//using System.Text;


//namespace EventBookingSystem
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.

//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            builder.Services.AddDbContext<EventDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EventConnectionString")));
//            builder.Services.AddScoped<IEventRepositories, EventRepositories>();
//            builder.Services.AddScoped<IEventService, EventService>();
//            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
//            builder.Services.AddScoped<IBookingService, BookingService>();
//            builder.Services.AddScoped<IAuthService, AuthService>();
//            builder.Services.AddScoped<IUserRepository, UserRepository>();

//            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//    });


//            builder.Services.AddSwaggerGen(options =>
//            {
//                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//                {
//                    Name = "Authorization",
//                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
//                    Scheme = "Bearer",
//                    BearerFormat = "JWT",
//                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//                    Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: Bearer {token}\"",
//                });

//                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//    {
//        {
//            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//            {
//                Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                {
//                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//            });





//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();
//            app.UseMiddleware<JwtMiddleware>();
//            app.UseAuthentication();
//            app.UseAuthorization();


//            app.MapControllers();

//            app.Run();
//        }
//    }
//}



// new


using EventBookingSystem.Data;
using EventBookingSystem.Repositories.Implementation;
using EventBookingSystem.Repositories.Interfaces;
using EventBookingSystem.Services.Implementation;
using EventBookingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

// Custom JWT Middleware & Authentication
using EventBookingSystem.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EventBookingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string publicKeyText = File.ReadAllText("Keys/public.pem");

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and your token.\n\nExample: Bearer {token}"
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

            builder.Services.AddDbContext<EventDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EventConnectionString")));

            builder.Services.AddScoped<IEventRepositories, EventRepositories>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Register custom JWT validation components
            builder.Services.AddSingleton<JwtSecurityTokenHandlerWrapper>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
            });

            // Register custom authentication handler (pass-through based on context)
            builder.Services
                .AddAuthentication("CustomScheme")
                .AddScheme<AuthenticationSchemeOptions, PassThroughAuthenticationHandler>("CustomScheme", null);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<JwtMiddleware>(); // Custom JWT validation
            app.UseAuthentication();            // Use our custom scheme
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
