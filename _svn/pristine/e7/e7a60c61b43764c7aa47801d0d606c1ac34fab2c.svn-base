using System;
using System.Data;
using System.Reflection;
using System.Collections;

namespace TinySyringe.Common
{
	public interface IRegister
	{
		IRegister To(Type serviceType);        
        IRegister Callin(CreationFunc creationFunc);
        IRegister Callback(PostFunc postFunc);
		IRegister Remove();				
		IRegister Singleton();
		IRegister PropertiesAutowired();
		IRegister UsingConstructor(params Type[] argTypes);        
	}

	public class Register : IRegister
	{
		public Register(Hashtable injectionHash, Type implementType)
		{
            this.implementType = implementType;
			this.injectionHash = injectionHash;
		}

		private Hashtable injectionHash;
        private Type implementType;		

		private InjectionInfo GetInjectionInfo(Type implementType, Type serviceType)
		{
			ConstructorInfo[] constInfos = serviceType.GetConstructors();
			InjectionInfo injectionInfo = new InjectionInfo(implementType, serviceType);

			if (constInfos.Length == 1)
			{
				injectionInfo.ServiceConstructor = constInfos[0];
			}

			return injectionInfo;
		}

		public IRegister To(Type serviceType)
		{
            injectionHash[implementType] = GetInjectionInfo(implementType, serviceType);
			
			return this;
		}

        public IRegister Callin(CreationFunc creationFunc)
		{
            if (!injectionHash.ContainsKey(implementType)) return this;
            (injectionHash[implementType] as InjectionInfo).CreationFunc = creationFunc;

            return this;
		}

		public IRegister Remove()
		{
            if (!injectionHash.ContainsKey(implementType)) return this;
            injectionHash.Remove(implementType);

			return this;
		}

		public IRegister Singleton()
		{
            if (!injectionHash.ContainsKey(implementType)) return this;
            (injectionHash[implementType] as InjectionInfo).IsSingleton = true;

			return this;
		}
		public IRegister PropertiesAutowired()
		{
            if (!injectionHash.ContainsKey(implementType)) return this;
            (injectionHash[implementType] as InjectionInfo).IsPropertiesAutowired = true;

			return this;
		}

		public IRegister UsingConstructor(params Type[] argTypes)
		{
            if (!injectionHash.ContainsKey(implementType)) return this;
            InjectionInfo injectionInfo = injectionHash[implementType] as InjectionInfo;
            ConstructorInfo constInfo = injectionInfo.ServiceType.GetConstructor(argTypes);

            if (constInfo != null) injectionInfo.ServiceConstructor = constInfo;

			return this;
		}

        public IRegister Callback(PostFunc postFunc)
        {
            if (!injectionHash.ContainsKey(implementType)) return this;
            InjectionInfo injectionInfo = injectionHash[implementType] as InjectionInfo;
            injectionInfo.PostFunc = postFunc;

            return this;
        }
    }
}