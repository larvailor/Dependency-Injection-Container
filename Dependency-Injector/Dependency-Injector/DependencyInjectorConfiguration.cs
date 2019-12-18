using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injector
{
    public class DependencyInjectorConfiguration
    {
        // variables
        public Dictionary<Type, List<Implementation>> dDepImpl { get; private set; } 



        // methods
        public DependencyInjectorConfiguration()
        {
            dDepImpl = new Dictionary<Type, List<Implementation>>();
        }



        public void Register<TDependency, TImplementation>(bool singleton = false)
        {
            Register(typeof(TDependency), typeof(TImplementation), singleton);
        }



        public void Register(Type tDependency, Type tImplementation, bool singleton = false)
        {
            if (tImplementation.IsAbstract)
            {
                throw new ArgumentException("Register failed. Implementation could not be abstract");
            }

            if (!tDependency.IsAssignableFrom(tImplementation)
                && !tDependency.IsGenericTypeDefinition
                && !tImplementation.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Register failed. Dependency is not assignable from implementation");
            }

            List<Implementation> implsForSpecificDependency;
            if (!dDepImpl.TryGetValue(tDependency, out implsForSpecificDependency))
            {
                implsForSpecificDependency = new List<Implementation>();
                dDepImpl[tDependency] = implsForSpecificDependency;
            }
            implsForSpecificDependency.Add(new Implementation(tImplementation, singleton));
        }



        public List<Implementation> GetImplementationsForDependency(Type tDependency)
        {
            if (dDepImpl.ContainsKey(tDependency))
            {
                return dDepImpl[tDependency];
            }
            else
            {
                return null;
            }
        }
    }
}
