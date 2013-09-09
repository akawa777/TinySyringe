using System;
using System.Data;
using System.Reflection;
using System.Collections;

namespace TinySyringe.Common
{
    public class InjectionInfo
    {
        public InjectionInfo(Type implementType, Type serviceType)
        {
            this.implementType = implementType;
            this.serviceType = serviceType;            
        }

        public Type ImplementType
        {
            get
            {
                return implementType;
            }
        }

		public Type ServiceType
		{
			get
			{
				return serviceType;
			}
		}

        public ConstructorInfo ServiceConstructor 
        { 
            get
            {
                return serviceConstructor;
            }
			set
			{
				serviceConstructor = value;				
			}
        }

        public CreationFunc CreationFunc
        {
            get
            {
                return creationFunc;
            }
			set
			{
                creationFunc = value;
			}
        }

        public object Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        public bool IsSingleton
        {
            get
            {
                return isSingleton;
            }
            set
            {
                isSingleton = value;
            }
        }

		public bool IsPropertiesAutowired
		{
			get
			{
				return isPropertiesAutowired;
			}
			set
			{
				isPropertiesAutowired = value;
			}
		}

        public PostFunc PostFunc
        {
            get
            {
                return postFunc;
            }
            set
            {
                postFunc = value;
            }
        }

        private Type implementType;
		private Type serviceType;
        private ConstructorInfo serviceConstructor;
        private CreationFunc creationFunc = null;
        private object instance = null;
        private bool isSingleton;
		private bool isPropertiesAutowired;
        private PostFunc postFunc;		
    }
}