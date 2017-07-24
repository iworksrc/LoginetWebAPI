using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using LoginetWebAPI.Services;

namespace LoginetWebAPI.Util
{
    public class Ninjecter: IDependencyResolver
    {

        private IKernel kernel;

        public Ninjecter(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IDataService>().To<DataService>();
        }
    }
}