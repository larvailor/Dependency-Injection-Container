using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injector
{
    public class Implementation
    {
        // variables
        public Type Type { get; private set; }
        public bool IsSingleton { get; private set; }
        public object Singleton { get; set; }


        // methods
        public Implementation(Type tImplementation, bool isSingleton)
        {
            Type = tImplementation;
            IsSingleton = isSingleton;
            Singleton = null;
        }
    }
}
