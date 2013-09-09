using System;
using System.Data;
using System.Reflection;
using System.Collections;

namespace TinySyringe
{
	public class ParameterList
	{
        private Hashtable paramHash = new Hashtable();

		public object this[Type implementType, string name]
		{
			get
			{
				return (paramHash[implementType.FullName + "," + name] as Parameter).Value;
			
			}
			set
			{
				paramHash[implementType.FullName + "," + name] = new Parameter(implementType, name, value);
			}
		}

		public bool ContainsKey(Type implementType, string name)
		{
			return paramHash.ContainsKey(implementType.FullName + "," + name);
		}

		public Parameter[] ToArray()
		{
			Parameter[] parameters = new Parameter[paramHash.Count];

			int index = 0;
			foreach (Parameter param in paramHash.Values)
			{
				parameters[index] = param;
				index++;
			}

			return parameters;
		}
    }

	public class Parameter
	{
		public Parameter(Type implementType, string name, object value)
		{
			this.implementType = implementType;
			this.name = name;
			this.value = value;
		}

		public Type ImplementType
		{
			get{ return implementType; }
			set{ implementType = value; }
		}

		public string Name
		{
			get{ return name; }
			set{ name = value; }
		}

		public object Value
		{
			get{ return value; }
			set{ this.value = value; }
		}

		private Type implementType;
		private string name;
		private object value;
	}
    
}