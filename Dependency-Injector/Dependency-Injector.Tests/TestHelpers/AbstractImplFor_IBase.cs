using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injector.Tests.TestHelpers
{
    public abstract class AbstractImplFor_IBase : IBase
    {
        public void BaseMethod()
        {
            throw new NotImplementedException();
        }
    }
}
