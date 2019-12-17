using System;
using System.Collections;
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



        public TDependency Resolve<TDependency>()
        {
            return (TDependency)Resolve(typeof(TDependency));
        }



        private object Resolve(Type tDependency)
        {
            object result = null;

            if (!DiConfig.dDepImpl.Any())
            {
                return result;
            }

            if (DiConfig.dDepImpl.ContainsKey(tDependency))
            {
                var implementations = new List<Implementation>(DiConfig.GetImplementationsForDependency(tDependency));
                if (implementations.Any())
                {
                    result = HandleSingletonCase(implementations[0]);
                }
            }
            else
            {
                result = CreateUsingConstructor(tDependency);
            }
   
            return result;
        }



        private object HandleSingletonCase(Implementation implementation)
        {
            object result = null;
            if (implementation.IsSingleton)
            {
                if (implementation.Singleton != null)
                {
                    result = implementation.Singleton;
                }
                else
                {
                    result = Resolve(implementation.Type);
                    implementation.Singleton = result;
                }
            }
            else
            {
                result = Resolve(implementation.Type);
            }
            
            return result;
        }



        private object CreateUsingConstructor(Type type)
        {
            object result = null;

            var constructorsInfo = type.GetConstructors();
            foreach (var constructorInfo in constructorsInfo)
            {
                var parameters = new List<object>();

                try
                {
                    var paramsInfo = constructorInfo.GetParameters();
                    foreach (var paramInfo in paramsInfo)
                    {
                        parameters.Add(Resolve(paramInfo.ParameterType));
                    }

                    result = Activator.CreateInstance(type, parameters.ToArray());
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch
                {
                    // do nothing 
                }
            }

            return result;
        }
    }
}
