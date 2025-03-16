
using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace CarRental
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(optios =>
            optios.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddProblemDetails(options =>
            //{
            //    options.CustomizeProblemDetails = context =>
            //    {
            //        context.ProblemDetails.Instance =
            //        $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

            //        context.ProblemDetails.Extensions.TryAdd("RequestId", context.HttpContext.TraceIdentifier);

            //        var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;

            //        context.ProblemDetails.Extensions.TryAdd("TraceId", activity?.Id);
            //    };
            //});

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
