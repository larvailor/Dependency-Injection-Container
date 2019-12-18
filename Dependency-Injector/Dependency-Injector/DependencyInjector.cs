using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injector
{
    public class DependencyInjector
    {
        // variables
        public DependencyInjectorConfiguration DiConfig { get; private set; }
        public Stack<Type> RecursionStack { get; private set; }



        // methods
        public DependencyInjector(DependencyInjectorConfiguration diConfig)
        {
            DiConfig = diConfig;
            RecursionStack = new Stack<Type>();
        }



        public TDependency Resolve<TDependency>()
        {
            return (TDependency)Resolve(typeof(TDependency));
        }



        private object Resolve(Type tDependency)
        {
            if (RecursionStack.Contains(tDependency))
            {
                throw new StackOverflowException("Infinite recursion detected");
            }
            RecursionStack.Push(tDependency);

            if (!DiConfig.dDepImpl.Any())
            {
                return null;
            }

            // resolving
            object result = null;

            if (typeof(IEnumerable).IsAssignableFrom(tDependency))
            {
                Type argumentType = tDependency.GetGenericArguments()[0];
                var implementations = new List<Implementation>(DiConfig.GetImplementationsForDependency(argumentType));
                var createdArguments = (object[])Activator.CreateInstance(argumentType.MakeArrayType(), new object[] { implementations.Count });
                for (var i = 0; i < implementations.Count; i++)
                {
                    createdArguments[i] = HandleSingletonCase(implementations[i]);
                }
                result = createdArguments;
            }
            else
            {
                if (DiConfig.dDepImpl.ContainsKey(tDependency))
                {
                    var implementations = new List<Implementation>(DiConfig.GetImplementationsForDependency(tDependency));
                    if (implementations.Any())
                    {
                        result = HandleSingletonCase(implementations[0]);
                    }
                }
                else
                {
                    result = CreateUsingConstructor(tDependency);
                }
            }

            RecursionStack.Pop();

            return result;
        }



        private object HandleSingletonCase(Implementation implementation)
        {
            object result = null;
            if (implementation.IsSingleton)
            {
                if (implementation.Singleton != null)
                {
                    result = implementation.Singleton;
                }
                else
                {
                    result = Resolve(implementation.Type);
                    implementation.Singleton = result;
                }
            }
            else
            {
                result = Resolve(implementation.Type);
            }
            
            return result;
        }



        private object CreateUsingConstructor(Type type)
        {
            object result = null;

            var constructorsInfo = type.GetConstructors();
            foreach (var constructorInfo in constructorsInfo)
            {
                var parameters = new List<object>();

                try
                {
                    var paramsInfo = constructorInfo.GetParameters();
                    foreach (var paramInfo in paramsInfo)
                    {
                        parameters.Add(Resolve(paramInfo.ParameterType));
                    }

                    result = Activator.CreateInstance(type, parameters.ToArray());
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (StackOverflowException e) // for handling recursion
                {
                    throw e;
                }
                catch
                {
                    // do nothing 
                }
            }

            return result;
        }
    }
}
