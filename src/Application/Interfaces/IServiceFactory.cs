using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXAM_SYSTEM_API.Application.Interfaces
{
    public interface IServiceFactory
    {
        IGenericCrudService<T> GetService<T>() where T : class;
    }
}
