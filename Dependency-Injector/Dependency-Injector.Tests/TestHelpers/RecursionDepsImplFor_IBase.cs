using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injector.Tests.TestHelpers
{
    public class RecursionDepsImplFor_IBase : IBase
    {
        // variables
        public IBase RecursiveField { get; private set; }



        // methods
        public RecursionDepsImplFor_IBase(IBase iBase)
        {
            RecursiveField = iBase;
        }



        public void BaseMethod()
        {
            throw new NotImplementedException();
        }
    }
}
