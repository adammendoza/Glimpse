using System.Collections.Generic;

namespace Glimpse.Core.Framework
{
    public interface IServiceLocator
    {
        T GetInstance<T>() where T : class;
        
        ICollection<T> GetAllInstances<T>() where T : class;
    }
}