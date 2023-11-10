using FurnituresServiceDatabase.Data;
using FurnituresServiceDatabase.Interfaces;
using FurnituresServiceDatabase.Repository;
using FurnituresServiceService.Interfaces;
using FurnituresServiceService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FurnituresService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

			builder.Services.AddScoped<IFurnituresRepository, FurnituresRepository>();
			builder.Services.AddScoped<ICategoriesRepository, CategoryRepository>();
			builder.Services.AddScoped<IUsersRepository, UsersRepository>();
			builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
			builder.Services.AddScoped<ICartRepository, CartRepository>();

			builder.Services.AddScoped<IFurnitureService, FurnitureService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICartService, CartService>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false) //changed from true
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "Manager", "Customer" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                if (await userManager.FindByEmailAsync("admin@admin.com") == null)
                {
                    IdentityUser admin = new IdentityUser()
                    {
                        Email = "admin@admin.com",
                        UserName = "admin@admin.com",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(admin, "Admin123@");
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            app.Run();
        }
    }
}