using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injector.Tests.TestHelpers
{
    public interface IBaseWithDependency<out T> where T : IEnumerable
    {
        void BaseMethod2();
    }
}
