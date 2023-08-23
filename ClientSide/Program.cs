using ClientSide.Repositories.Data;
using ClientSide.Repositories.Interface;
using ClientSide.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;

namespace ClientSide
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddControllersWithViews();
			builder.Services.AddSession();

			builder.Services.AddScoped(typeof(IGeneralRepository<,>), typeof(GeneralRepository<,>));
			builder.Services.AddScoped<IOvertimeRepository, OvertimeRepository>();
			builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
			builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
			builder.Services.AddScoped<IAccountRoleRepository, AccountRoleRepository>();
			builder.Services.AddScoped<IEmployeeLevelRepository, EmployeeLevelRepository>();
			builder.Services.AddScoped<HomeRepository>();

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(options => 
					{
						options.RequireHttpsMetadata = false;
						options.SaveToken = true;
						options.TokenValidationParameters = new TokenValidationParameters
						{
						   ValidateIssuer = true,
						   ValidateAudience = true,
						   ValidateLifetime = true,
						   ValidateIssuerSigningKey = true,
						   ValidIssuer = builder.Configuration["Jwt:Issuer"],
						   ValidAudience = builder.Configuration["Jwt:Audience"],
						   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
						   ClockSkew = TimeSpan.Zero
						 };
					});
			
			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

			app.UseStatusCodePages(async context =>
			{
				var response = context.HttpContext.Response;

				if (response.StatusCode.Equals((int)HttpStatusCode.Unauthorized))
				{
					response.Redirect("/unauthorized");
				}
				else if (response.StatusCode.Equals((int)HttpStatusCode.NotFound))
				{
					response.Redirect("/notfound");
				}
				else if (response.StatusCode.Equals((int)HttpStatusCode.Forbidden))
				{
					response.Redirect("/forbidden");
				}
			});
			app.UseSession();

			app.Use(async (context, next) =>
			{
				var JWToken = context.Session.GetString("JWToken");

				if (!string.IsNullOrEmpty(JWToken))
				{
					context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
				}
				await next();
			});

			app.UseAuthentication();
			app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Dashboard}/{id?}");

            app.Run();
        }
    }
}