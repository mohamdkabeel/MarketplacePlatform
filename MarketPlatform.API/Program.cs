using Marketplace.Application.IServices.AdminReportService;
using Marketplace.Application.IServices.Auth;
using Marketplace.Application.IServices.Category;
using Marketplace.Application.IServices.Email;
using Marketplace.Application.IServices.FileServices;
using Marketplace.Application.IServices.Notification;
using Marketplace.Application.IServices.Order;
using Marketplace.Application.IServices.Product;
using Marketplace.Application.IServices.Reviews;
using Marketplace.Application.IServices.WishList;
using Marketplace.Core.Entites;
using Marketplace.Infrastructure.Data;
using Marketplace.Infrastructure.Services.AdminServices;
using Marketplace.Infrastructure.Services.Auth;
using Marketplace.Infrastructure.Services.Cart;
using Marketplace.Infrastructure.Services.Category;
using Marketplace.Infrastructure.Services.Email;
using Marketplace.Infrastructure.Services.FileService;
using Marketplace.Infrastructure.Services.Notification;
using Marketplace.Infrastructure.Services.Order;
using Marketplace.Infrastructure.Services.Product;
using Marketplace.Infrastructure.Services.Reviews;
using Marketplace.Infrastructure.Services.Wishlist;
using Marketplace.Infrastructure.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text; // Ensure this is included



namespace MarketPlatform.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");


            //dbcontext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionstring);
            });

            // Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;


                options.SignIn.RequireConfirmedAccount = false;
                //options.SignIn.RequireConfirmedEmail = false;
                //options.SignIn.RequireConfirmedPhoneNumber = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                options.Lockout.MaxFailedAccessAttempts = 7;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // jwt 
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var SecreteKey = jwtSettings.GetValue<string>("SecretKey");

            builder.Services.AddAuthentication(opitons =>
            {

                opitons.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opitons.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issure"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecreteKey))
                };
            });





            builder.Services.AddScoped<IAuthServices , AuthService>();
            builder.Services.AddScoped<IProductServices, ProductService>();
            builder.Services.AddScoped<ICategoryService , CategoryService>();
            builder.Services.AddScoped<IFileServices, FileServices>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IWishlistService, WishlistService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IAdminReportService, AdminReportService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();




            builder.Services.AddSignalR();




            // Auterization 
            builder.Services.AddAuthorization();
            //Access Controllers
            //builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddAutoMapper(typeof(Program));


            //builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

            var app = builder.Build();
            //app.MapGet("/", () => "API is running...");


            // RoleManager + user manager is services scoped so : 
            using (var scope = app.Services.CreateScope()) 
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roles = new[] { "Admin", "Seller", "Custmor" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }


                var adminEmail = "Kabeelmohamed22@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = adminEmail,
                        FirstName = "Platform0",
                        LastName = "admin"

                    };

                    var result = await userManager.CreateAsync(adminUser, "123456789mM#");

                    if (result.Succeeded)
                    {
                       await userManager.AddToRoleAsync(adminUser, "admin");
                    }
                }
            } 


                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseAuthentication(); // 1
            app.UseAuthorization(); // 2

            app.UseStaticFiles(); // To Access Image

            app.MapHub<NotificationHub>("/notifications");
            app.MapControllers();
            app.Run();
        }
    }
}
