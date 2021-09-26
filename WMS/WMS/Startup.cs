using WMS.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WMS.Services;
using WMS.Models;
using WMS.DTO;
using WMS.Utils;
using WMS.Extensions;

namespace WMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // подключение к SQL Server
            services.AddDbContext<WmsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("WmsDbContext")));
            services.AddScoped<DbContext, WmsDbContext>();
            // services.AddSingleton<DbContext, WmsDbContext>();

            // регистрация автомаппера
            services.AddAutoMapper(typeof(Startup));

            // регистрация сервисов
            // services.AddTransient<IDataService<Item, ItemDto>, ServiceBase<Item, ItemDto>>();
            // services.AddTransient<IReadService<Warehouse>, ServiceBase<Warehouse, WarehouseDto>>();
            // services.AddTransient<DataResult<object>, DataResult<object>>();

            services.RegisterDataService<ServiceBase<Item, ItemDto>, Item, ItemDto>();
            services.RegisterDataService<ServiceBase<Warehouse, WarehouseDto>, Warehouse, WarehouseDto>();
            services.RegisterDataService<WarehouseItemsService, WarehouseItem, WarehouseItemDto>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // определение маршрутов
                endpoints.MapControllers();
            });
        }
    }
}
