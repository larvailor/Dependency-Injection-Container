using System;
using System.Collections.Generic;
using Dependency_Injector.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dependency_Injector.Tests
{
    [TestClass]
    public class DiTests
    {
        [TestMethod]
        public void DIConfig_ShouldRegister_IfCorrect()
        {
            var diConfig = new DependencyInjectorConfiguration();
            diConfig.Register<IBase, ImplFor_IBase>();

            var keys = new List<Type>(diConfig.dDepImpl.Keys);
            var values = new List<List<Implementation>>(diConfig.dDepImpl.Values);

            Assert.AreEqual(1, keys.Count);
            Assert.AreEqual("IBase", keys[0].Name);
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(1, values[0].Count);
            Assert.AreEqual("ImplFor_IBase", values[0][0].Type.Name);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DIConfig_ShouldThrow_ArgumentException_IfImplementationIsAbstract()
        {
            var diConfig = new DependencyInjectorConfiguration();
            diConfig.Register<IBase, AbstractImplFor_IBase>();
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DIConfig_ShouldThrow_ArgumentException_IfDependencyIsNotAssignableFromImplementation()
        {
            var diConfig = new DependencyInjectorConfiguration();
            diConfig.Register<IBase, ClassWithoutImpl>();
        }
    }
}
