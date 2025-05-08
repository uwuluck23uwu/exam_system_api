using Microsoft.Extensions.DependencyInjection;
using EXAM_SYSTEM_API.Application.Interfaces;

namespace EXAM_SYSTEM_API.Application.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IGenericCrudService<T> GetService<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<IGenericCrudService<T>>();
        }
    }
}
