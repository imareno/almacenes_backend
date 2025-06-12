
using Almacen._1.Api.Estructure;
using Almacen._1.Api.Extensions;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureDbContext(Configuration);
            services.ConfigureAutoMapper();
            services.ConfigurationValidation();
            services.ConfigureAppInterfaces();
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
            services.ConfigureAuthentication();
            services.AddControllers();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");
            app.UseMiddleware<GlobalHandlerExceptions>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}