using APIs.Context;
using APIs.Contract;
using APIs.Repositories;
using APIs.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            // create route
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<PayrollOvertimeContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAccountRoleRepository, AccountRoleRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IEmployeeLevelRepository, EmployeeLevelRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IOvertimeRepository, OvertimeRepository>();
            builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddSingleton(typeof(IMapper<,>), typeof(Mapper<,>));
            
            //email configure
            builder.Services.AddTransient<IEmailService, MailService>(_ => new MailService(
                smtpServer: builder.Configuration["Email:SmtpServer"],
                smtpPort: int.Parse(builder.Configuration["Email:SmtpPort"]),
                fromEmailAddress: builder.Configuration["Email:FromEmailAddress"]
            ));
            // Add authentication to the container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.SaveToken = true;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {

                           ValidateAudience = false,
                           ValidAudience = builder.Configuration["JWT:Audience"],
                           ValidateIssuer = false,
                           ValidIssuer = builder.Configuration["JWT:Issuer"],
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                           ValidateLifetime = true,
                           ClockSkew = TimeSpan.Zero
                       };
                   });
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MCC 78 Kelompok Yamaha",
                    Description = "Semakin Di Depan"
                });
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                     }
                });
            });
            // set up cors default
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();

            app.MapControllers();

            app.Run();
        }
    }
}