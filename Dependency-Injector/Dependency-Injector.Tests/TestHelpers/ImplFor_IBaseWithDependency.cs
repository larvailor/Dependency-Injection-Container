using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injector.Tests.TestHelpers
{
    public class ImplFor_IBaseWithDependency<T> : IBaseWithDependency<T> where T : ArrayList
    {
        public void BaseMethod2()
        {
            throw new NotImplementedException();
        }
    }
}
