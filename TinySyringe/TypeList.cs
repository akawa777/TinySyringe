using System;
using System.Data;
using System.Reflection;
using System.Collections;

namespace TinySyringe
{
	public class TypeList
	{
        public TypeList(Hashtable typeHash)
        {
            this.typeHash = typeHash;
        }

        private Hashtable typeHash = new Hashtable();

		public object this[Type implementType]
		{
			get
			{
                return typeHash[implementType] as Type;			
			}
		}

		public bool ContainsKey(Type implementType)
		{
            return typeHash.ContainsKey(implementType);
		}

        public Type[] GetImplementTypes()
        {
            ArrayList keyList = new ArrayList();
            foreach (Type key in typeHash.Keys)
            {
                keyList.Add(key);
            }
            return keyList.ToArray(typeof(Type)) as Type[];
        }

        public Type[] GetServiceTypes()
        {
            ArrayList valueList = new ArrayList();
            foreach (Type value in typeHash.Values)
            {
                valueList.Add(value);
            }
            return valueList.ToArray(typeof(Type)) as Type[];
        }
    }   
}