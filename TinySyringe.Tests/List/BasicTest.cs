using System;
using NUnit.Framework;
using TinySyringe;
using System.Collections;

namespace TinySyringe.Tests.List
{	
	[TestFixture]
	public class BasicTest
	{		
		public BasicTest()
		{
			IContainer container = new Container();

			container.Set(typeof(IAnimal)).To(typeof(Dog));
			container.Set(typeof(IBehavior)).To(typeof(Behavior));
			container.Set(typeof(IBark)).To(typeof(Bark));
			container.Set(typeof(IWalk)).To(typeof(Walk));			

			DependencyResolver.Container = container;
		}

		[Test]
		public void BasicTest_ConstructorInjection()
		{
			ParameterList args = new ParameterList();
			args[typeof(IAnimal), "name"] = "POCHI";

			IAnimal animal = DependencyResolver.Get(typeof(IAnimal), args) as IAnimal;

			animal.Bark();
			animal.Wark();			

			Assert.AreEqual(typeof(Dog), animal.GetType());
			Assert.AreEqual(args[typeof(IAnimal), "name"].ToString(), (animal as Dog).Name);

			args[typeof(IAnimal), "behavior"] = new Behavior2();

			animal = DependencyResolver.Get(typeof(IAnimal), args) as IAnimal;

			Assert.AreEqual(args[typeof(IAnimal), "behavior"], (animal as Dog).GetBehavior());			
		}

		[Test]
		public void BasicTest_LoadModule()
		{	
			DependencyResolver.Container.LoadModules(new Module());

			ParameterList args = new ParameterList();
			args[typeof(IAnimal), "name"] = "BIG POCHI";

			IAnimal animal = DependencyResolver.Get(typeof(IAnimal), args) as IAnimal;

			animal.Bark();
			animal.Wark();			

			Assert.AreEqual(typeof(BigDog), animal.GetType());
			Assert.AreEqual(args[typeof(IAnimal), "name"].ToString(), (animal as BigDog).Name);
		}

		[Test]
		public void BasicTest_Remove_IsSingleton()
		{				
			DependencyResolver.Container.Set(typeof(Dog)).Remove();
			DependencyResolver.Container.Set(typeof(IAnimal)).To(typeof(Dog));
			DependencyResolver.Container.Set(typeof(IAnimal)).Singleton();

			ParameterList args = new ParameterList();
			args[typeof(IAnimal), "name"] = "POCHI";

			IAnimal animal = DependencyResolver.Get(typeof(IAnimal), args) as IAnimal;

			animal.Bark();
			animal.Wark();			

			IAnimal animal2 = DependencyResolver.Get(typeof(IAnimal), args) as IAnimal;

			animal2.Bark();
			animal2.Wark();			

			Assert.AreEqual(animal, animal2);			
		}

		[Test]
		public void BasicTest_PropertiesAutowired_IsUsingConstructor()
		{	
			DependencyResolver.Container.Set(typeof(IBehavior)).To(typeof(Behavior2)).PropertiesAutowired();

			DependencyResolver.Container.Set(typeof(BigDog)).To(typeof(BigDog));
			DependencyResolver.Container.Set(typeof(BigDog)).UsingConstructor(typeof(string)).PropertiesAutowired();			

			ParameterList args = new ParameterList();
			args[typeof(BigDog), "name"] = "BIG POCHI";

			IAnimal animal = DependencyResolver.Get(typeof(BigDog), args) as IAnimal;

			animal.Bark();
			animal.Wark();			

			Assert.AreEqual(typeof(BigDog), animal.GetType());
			Assert.AreEqual(args[typeof(BigDog), "name"].ToString(), (animal as BigDog).Name);		
		}

        [Test]
        public void BasicTest_CreationFunc_PostFunc()
        {			
            DependencyResolver.Container.Set(typeof(Dog)).To(typeof(BigDog)).Callin(new CreationFunc(CreateBigDog)).Callback(new PostFunc(PostBigDog));                        

            IAnimal animal = DependencyResolver.Get(typeof(Dog)) as IAnimal;

            Assert.AreEqual(typeof(BigDog), animal.GetType());
            Assert.AreEqual("BIG POCHI", (animal as BigDog).Name);
        }

        private object CreateBigDog(Type implementType, IContainer container, ParameterList parameterList)
        {            
            return new BigDog(string.Empty);
        }

        private void PostBigDog(Type implementType, object instance, IContainer container, ParameterList parameterList)
        {
            (instance as Dog).Name = "BIG POCHI";
        }
	}
}
