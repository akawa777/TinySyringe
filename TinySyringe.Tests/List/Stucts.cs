using System;

namespace TinySyringe.Tests.List
{
	public interface IAnimal
	{
		void Bark();
		void Wark();
	}

	public interface IBehavior
	{
		void Bark();
		void Wark();
	}
	
	public interface IBark
	{
		void Do();
	}
	
	public interface IWalk
	{
		void Do();
	}

	public interface IRun
	{
		void Do();
	}
	
	public class Bark : IBark
	{
		public void Do()
		{
		}
	}
	
	public class Walk : IWalk
	{
		public void Do()			
		{
		}
	}
	
	public class Behavior : IBehavior
	{
		public Behavior()
		{
			
		}
        
		public Behavior(IBark bark, IWalk walk)
		{			
			this.bark = bark;
			this.walk = walk;
		}

		private IBark bark;
		private IWalk walk;

		public void Bark()
		{
			bark.Do();
		}

		public void Wark()
		{
			walk.Do();
		}
	}

	public class Behavior2 : IBehavior
	{
		private IBark bark;
		private IWalk walk;

		public IBark BarkObj
		{
			set { bark = value; }
		}

		public IWalk WarkObj
		{
			set { walk = value; }
		}

		public void Bark()
		{
			bark.Do();
		}

		public void Wark()
		{
			walk.Do();
		}
	}
	
	public class Dog : IAnimal
	{
		public Dog()
		{
			
		}
        
		public Dog(string name, IBehavior behavior)
		{
			Name = name;
			this.behavior = behavior;			
		}

		protected IBehavior behavior;		

		public void Bark()
		{
			behavior.Bark();
		}

		public void Wark()
		{
			behavior.Wark();
		}

		public string Name;

		public IBehavior GetBehavior()
		{
			return behavior;
		}
	}

	public class BigDog : Dog
	{
		public BigDog(string name) : base(name, null)
		{
		}

		public BigDog(string name, IBehavior behavior) : base(name, behavior)
		{
		}

		public IBehavior Behavior
		{
			set { behavior = value; }
		}
	}
	
	public class Module : IModule
	{
		public void Load(IContainer container)
		{			
			container.Set(typeof(IAnimal)).To(typeof(BigDog));			
		}
	}
}
