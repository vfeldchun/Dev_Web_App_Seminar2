
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using WebApi.Abstraction;
using WebApi.Models;
using WebApi.Repo;

namespace WebApi
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
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            var cfg = config.Build();

            builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
            {
                // Добавили строку подключения из файла appsettings.json
                cb.Register(c => new ProductContext(cfg.GetConnectionString("db"))).InstancePerDependency();
                cb.RegisterType<ProductRepository>().As<IProductRepository>();
            });
            // builder.Services.AddSingleton<IProductRepository, ProductRepository>();

            builder.Services.AddMemoryCache(o => o.TrackStatistics = true );

            var app = builder.Build();

            // Работа со статическими данными кеша
            var cacheStaticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "CacheStaticFiles");
            Directory.CreateDirectory(cacheStaticFilesPath);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(cacheStaticFilesPath),
                RequestPath = "/cache_static"
            });


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
