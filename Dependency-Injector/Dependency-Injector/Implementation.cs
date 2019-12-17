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


        // methods
        public Implementation(Type tImplementation)
        {
            Type = tImplementation;
        }
    }
}
