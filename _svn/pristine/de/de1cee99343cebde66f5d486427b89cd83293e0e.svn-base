using System;
using System.Data;
using System.Reflection;
using System.Collections;

namespace TinySyringe.Tests.List
{
    public class DependencyResolver
    {
        private static DependencyResolver instance;
        private IContainer container;

        private DependencyResolver()
        {

        }

        private static DependencyResolver Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DependencyResolver();
                }

                return instance;
            }
        }

        public static object Get(Type type)
        {
            return Instance.container.Get(type);
        }

        public static object Get(Type type, ParameterList argList)
        {
            return Instance.container.Get(type, argList);
        }

        public static IContainer Container
        {
            get
            {
                return Instance.container;
            }
            set
            {
                Instance.container = value;
            }
        }
    }
}