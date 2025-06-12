

namespace Almacen._1.Api.Extensions
{
    public static class ValidationExtension
    {
        public static void ConfigurationValidation(this IServiceCollection services)
        {
            //services.AddValidatorsFromAssemblyContaining<PersonValidator>();
            
            //services.AddControllers().AddFluentValidation(options => options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            //services.AddMvc(options => options.Filters.Add<HandlerValidation>()).ConfigureApiBehaviorOptions(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //});
        }
    }
}
