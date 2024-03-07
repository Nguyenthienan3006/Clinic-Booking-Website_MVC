using Microsoft.EntityFrameworkCore;
using Website_Mvc.Controllers;
using Website_Mvc.Models;
using Website_Mvc.Repositories;

namespace Website_Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ClinicBookingProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<CommonRepository>();
            builder.Services.AddScoped<PatientRepository>();

            //add session
            builder.Services.AddSession(); 

			// Add services to the container.
			builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

			app.UseSession(); // Thêm dòng này để kích hoạt Session

			app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}