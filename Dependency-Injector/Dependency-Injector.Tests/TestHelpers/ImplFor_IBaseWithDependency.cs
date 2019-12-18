using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injector.Tests.TestHelpers
{
    public class ImplFor_IBaseWithDependency<ImplFor_IBase> : IBaseWithDependency<ImplFor_IBase> where ImplFor_IBase : IBase
    {
        public void BaseMethod2()
        {
            throw new NotImplementedException();
        }
    }
}
