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
                var implementations = new List<Implementation>();
                if (tDependency.IsGenericType && DiConfig.dDepImpl.ContainsKey(tDependency.GetGenericTypeDefinition()))
                {
                    implementations = new List<Implementation>(DiConfig.GetImplementationsForDependency(tDependency.GetGenericTypeDefinition()));
                }
                else
                {
                    if (DiConfig.dDepImpl.ContainsKey(tDependency))
                    {
                        implementations = new List<Implementation>(DiConfig.GetImplementationsForDependency(tDependency));
                    }
                }

                if (implementations.Any())
                {
                    result = HandleSingletonCase(implementations[0]);
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
            if (implementation.IsSingleton)
            {
                if (implementation.Singleton == null)
                {
                    lock (implementation)
                    {
                        if (implementation.Singleton == null)
                        {
                            implementation.Singleton = Resolve(implementation.Type);
                            return implementation.Singleton;
                        }
                    }
                }
                return implementation.Singleton;
            }
            else
            {
                return Resolve(implementation.Type);
            }
        }



        private object CreateUsingConstructor(Type type)
        {
            object result = null;

            if (type.ContainsGenericParameters)
            {
                var genericArguments = type.GetGenericArguments();
                var genericParams = genericArguments.Select(dependency =>
                {   
                    var implementations = DiConfig.GetImplementationsForDependency(dependency.BaseType)?.ToArray();
                    if (implementations == null)
                    {
                        return dependency.BaseType;
                    }
                    else
                    {
                        return implementations.First().Type;
                    }
                }).ToArray();

                type = type.MakeGenericType(genericParams);
            }

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
