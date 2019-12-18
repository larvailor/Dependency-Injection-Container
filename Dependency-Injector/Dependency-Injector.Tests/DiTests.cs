using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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



        [TestMethod]
        public void DI_ShouldReturn_Null_IfNotRegistered()
        {
            var diConfig = new DependencyInjectorConfiguration();
            // register part skipped

            var di = new DependencyInjector(diConfig);
            var actual = di.Resolve<IBase>();

            Assert.AreEqual(null, actual);
        }



        [TestMethod]
        public void DI_ShouldResolve_IfCorrect()
        {
            var diConfig = new DependencyInjectorConfiguration();
            diConfig.Register<IBase, ImplFor_IBase>();

            var di = new DependencyInjector(diConfig);
            var actual = di.Resolve<IBase>();

            Assert.AreEqual(typeof(ImplFor_IBase), actual.GetType());
        }



        [TestMethod]
        public void DI_ShouldResolve_WithInnerDependencies()
        {
            var diConfig = new DependencyInjectorConfiguration();
            diConfig.Register<IBase, InnerDepsImplFor_IBase>();

            var di = new DependencyInjector(diConfig);
            var actual = di.Resolve<IBase>();

            Assert.AreEqual(typeof(InnerDepsImplFor_IBase), actual.GetType());
        }



        [TestMethod]
        [ExpectedException(typeof(StackOverflowException))]
        public void DI_ShouldThrow_StackOverflowException_IfRecursionDetected()
        {
            var diConfig = new DependencyInjectorConfiguration();
            diConfig.Register<IBase, RecursionDepsImplFor_IBase>();

            var di = new DependencyInjector(diConfig);
            var actual = di.Resolve<IBase>();
        }



        [TestMethod]
        public void DI_ShouldUse_NewInstance_WhenNotSingleton()
        {
            var diConfig = new DependencyInjectorConfiguration();
            diConfig.Register<IBase, ImplFor_IBase>(singleton: false);

            var di = new DependencyInjector(diConfig);
            var actual1 = di.Resolve<IBase>();
            var actual2 = di.Resolve<IBase>();

            Assert.AreNotSame(actual1, actual2);
        }



        [TestMethod]
        public void DI_ShouldUse_SameInstance_WhenSingleton()
        {
            var diConfig = new DependencyInjectorConfiguration();
            diConfig.Register<IBase, ImplFor_IBase>(singleton: true);

            var di = new DependencyInjector(diConfig);
            var actual1 = di.Resolve<IBase>();
            var actual2 = di.Resolve<IBase>();

            Assert.AreSame(actual1, actual2);
        }



        [TestMethod]
        public void DI_ShouldReturn_MultipleImplementations_ForOneDependency()
        {
            var diConfig = new DependencyInjectorConfiguration();
            diConfig.Register<IBase, ImplFor_IBase>();
            diConfig.Register<IBase, ImplFor_IBase_2>();

            var di = new DependencyInjector(diConfig);
            var actual = di.Resolve<IEnumerable<IBase>>();

            Assert.AreEqual(typeof(ImplFor_IBase), actual.First().GetType());
            Assert.AreEqual(typeof(ImplFor_IBase_2), actual.Last().GetType());
        }



        [TestMethod]
        public void DI_ShouldResolve_WhenGenericDependency()
        {
            //var diConfig = new DependencyInjectorConfiguration();
            //diConfig.Register<IBaseWithDependency<ImplFor_IBase>, ImplFor_IBaseWithDependency<ImplFor_IBase>>();

            //var di = new DependencyInjector(diConfig);
            //var actual = di.Resolve<IBaseWithDependency<ImplFor_IBase>>();

            //Assert.AreEqual(typeof(ImplFor_IBaseWithDependency<ImplFor_IBase>), actual.GetType());
        }



        [TestMethod]
        public void DI_ShouldResolve_WhenOpenGenericDependency()
        {
            var diConfig = new DependencyInjectorConfiguration();
            diConfig.Register(typeof(IBaseWithDependency<>), typeof(ImplFor_IBaseWithDependency<>));

            var di = new DependencyInjector(diConfig);
            var actual = di.Resolve<IBaseWithDependency<IEnumerable>>();

            Assert.AreEqual(typeof(ImplFor_IBaseWithDependency<ArrayList>), actual.GetType());
        }
    }
}
