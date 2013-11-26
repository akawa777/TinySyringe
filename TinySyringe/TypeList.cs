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
            return typeHash.Keys as Type[];
        }

        public Type[] GetServiceTypes()
        {
            return typeHash.Values as Type[];
        }
    }   
}