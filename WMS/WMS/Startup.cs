using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using WMS.DataAccess.Data;
using WMS.BusinessLogic.Extensions;
using WMS.BusinessLogic.Services;
using WMS.DataAccess.Models;
using WMS.BusinessLogic.DTO;
using WMS.BusinessLogic.Mappers;

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

            // services.AddDbContext<WmsDbContext>(options =>
            //     options.UseSqlServer(Configuration.GetConnectionString("WmsDbContext")));
            // services.AddScoped<DbContext, WmsDbContext>();

            // connect to postgres

            services.AddDbContext<WmsDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("WmsDbContextPostgres")));
            services.AddScoped<DbContext, WmsDbContext>();

            // регистрация автомаппера

            services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(typeof(WmsProfile));

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
