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

            // регистрация автомаппера

            services.AddAutoMapper(typeof(Startup));

            // регистрация сервисов

            services.RegisterDataService<ServiceBase<Warehouse, WarehouseDto>, Warehouse, WarehouseDto>();
            services.RegisterDataService<WarehouseItemsService, WarehouseItem, WarehouseItemDto>();
            services.RegisterDataService<ItemsService, Item, ItemDto>();
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
