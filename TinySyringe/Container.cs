using System;
using System.Data;
using System.Reflection;
using System.Collections;
using TinySyringe.Common;

namespace TinySyringe
{	
    public interface IContainer
    {   
		object Get(Type type);
		object Get(Type type, ParameterList parameterList);
		IRegister Set(Type implementType);
		void LoadModules(params IModule[] modules);
        TypeList GetTypeList();
    }

	public class Container : IContainer
	{
		public Container()
		{
			
		}

		private Hashtable injectionHash = new Hashtable();

		public object Get(Type type)
		{
			return Get(type, null);            
		}

		private bool Has(Type type)
		{
            return injectionHash.ContainsKey(type.FullName);
		}

		public object Get(Type type, ParameterList parameterList)
		{
            if (!injectionHash.ContainsKey(type.FullName))
			{
				return null;
			}

            InjectionInfo injectionInfo = injectionHash[type.FullName] as InjectionInfo;
			Type serviceType = injectionInfo.ServiceType;

			if (injectionInfo.Instance != null)
			{
				return injectionInfo.Instance;
			}			

			if (parameterList == null)
			{
				parameterList = new ParameterList();
			}

			object instance;

			if (injectionInfo.CreationFunc != null)
			{
                instance = injectionInfo.CreationFunc(type, this, parameterList);
			}
			else
			{
				ConstructorInfo constInfo = GetConstructorInfo(injectionInfo, parameterList);
				object[] parameters = GetConstructorParameterList(type, constInfo, parameterList);

				instance = constInfo.Invoke(parameters);
			}

			if (injectionInfo.IsSingleton)
			{
				injectionInfo.Instance = instance;
			}

			if (injectionInfo.IsPropertiesAutowired)
			{
				InjectProperty(instance, parameterList);
			}

            if (injectionInfo.PostFunc != null)
            {
                injectionInfo.PostFunc(type, instance, this, parameterList);
            }

			return instance;
		}

		private object[] GetConstructorParameterList(Type type, ConstructorInfo constInfo, ParameterList parameterList)
		{
			ParameterInfo[] paramInfos = constInfo.GetParameters();

			object[] parameters = new object[paramInfos.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo paramInfo = paramInfos[i];
				object param = null;

				if (parameterList.ContainsKey(type, paramInfo.Name) 
					&& IsCast(parameterList[type, paramInfo.Name].GetType(), paramInfo.ParameterType))
				{					
					param = parameterList[type, paramInfo.Name];
				}
				else
				{
					param = Get(paramInfo.ParameterType, parameterList); 
				}

				parameters[i] = param;
			}

			return parameters;
		}	
	
		private bool IsCast(Type type, Type castType)
		{
			return type.GetInterface(castType.FullName) != null
				|| type.IsSubclassOf(castType)
				|| type == castType ? true : false;
		}

		private void InjectProperty(object instance, ParameterList parameterList)
		{
			PropertyInfo[] propInfos = instance.GetType().GetProperties();

			foreach (PropertyInfo propInfo in propInfos)
			{
				if (!propInfo.CanWrite || !Has(propInfo.PropertyType)) continue;

				propInfo.SetValue(instance, Get(propInfo.PropertyType, parameterList), null);
			}
		}

		private ConstructorInfo GetConstructorInfo(InjectionInfo injectionInfo, ParameterList parameterList)
		{
			ConstructorInfo targetConstInfo = injectionInfo.ServiceConstructor;

			if (targetConstInfo != null) return targetConstInfo;

			int maxCount = 0;
			int count = 0;			

			foreach (ConstructorInfo constInfo in injectionInfo.ServiceType.GetConstructors())
			{
				count = 0;
				foreach (ParameterInfo paramInfo in constInfo.GetParameters())
				{
					if (parameterList.ContainsKey(injectionInfo.ImplementType, paramInfo.Name) 
						&& IsCast(parameterList[injectionInfo.ImplementType, paramInfo.Name].GetType(), paramInfo.ParameterType))
					{
						count++;
					}
					else if (Has(paramInfo.ParameterType))
					{
						count++;
					}
				}

				if (count > maxCount)
				{
					targetConstInfo = constInfo;
					maxCount = count;
				}
			}

			return targetConstInfo;
		}

		public IRegister Set(Type implementType)
		{
			return new Register(injectionHash, implementType);
		}

		public void LoadModules(params IModule[] modules)
		{            
			foreach (IModule module in modules)
			{
                module.Load(this);
			}
		}

        public TypeList GetTypeList()
        {
            Hashtable typeHash = new Hashtable();

            foreach (InjectionInfo injectionInfo in injectionHash.Values)
            {
                typeHash[injectionInfo.ImplementType] = injectionInfo.ServiceType;
            }

            return new TypeList(typeHash);
        }
    }
}