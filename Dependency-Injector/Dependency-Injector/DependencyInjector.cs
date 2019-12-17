using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injector
{
    public class DependencyInjector
    {
        // variables
        public DependencyInjectorConfiguration DiConfig { get; private set; }



        // methods
        public DependencyInjector(DependencyInjectorConfiguration diConfig)
        {
            DiConfig = diConfig;
        }



        public object Resolve<TDependency>()
        {
            Type tDependency = typeof(TDependency);

            object result = null;
            if (!DiConfig.dDepImpl.Any())
            {
                return result;
            }




            return result;
        }
    }
}
