using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injector.Tests.TestHelpers
{
    public class InnerDepsImplFor_IBase : IBase
    {
        public InnerDepsImplFor_IBase(ClassWithoutImpl cwi)
        {

        }



        public void BaseMethod()
        {
            throw new NotImplementedException();
        }
    }
}
