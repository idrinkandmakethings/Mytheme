using Microsoft.Extensions.DependencyInjection;
using Mytheme.Modal.Services;

namespace Mytheme.Modal
{
    public static class ServiceRegistrationExtension
    {
        public static IServiceCollection AddMythemeModal(this IServiceCollection services)
        {
            return services.AddScoped<IModalService, ModalService>();
        }
    }
}
