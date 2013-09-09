using System;
using NUnit.Framework;
using TinySyringe;
using System.Collections;

namespace TinySyringe.Tests.List
{	
	[TestFixture]
	public class PerformanceTest
	{		
		public PerformanceTest()
		{
			IContainer container = new Container();

			for (int i = 0; i < 10000; i++)
			{

				container.Set(typeof(IAnimal)).To(typeof(Dog));
				container.Set(typeof(IBehavior)).To(typeof(Behavior));
				container.Set(typeof(IBark)).To(typeof(Bark));
				container.Set(typeof(IWalk)).To(typeof(Walk));	

				container.Set(typeof(Behavior)).To(typeof(Behavior2));
				container.Set(typeof(Dog)).To(typeof(BigDog));
			}

			DependencyResolver.Container = container;
		}

		[Test]
		public void PerformanceTest_Test()
		{
			for (int i = 0; i < 10000; i++)
			{	
				IBehavior behavior = DependencyResolver.Get(typeof(IBehavior)) as IBehavior;
				IAnimal animal = DependencyResolver.Get(typeof(IAnimal)) as IAnimal;
			}

			Assert.AreEqual(1,1);
		}
	}
}
