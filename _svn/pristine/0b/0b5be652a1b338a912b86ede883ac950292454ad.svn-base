using System;
using NUnit.Framework;
using TinySyringe;
using System.Collections;

namespace TinySyringe.Tests
{	
	[TestFixture]
	public class GetStarted
    {
        #region initialization for test

        public GetStarted()
		{
            // initialization for test
			Global global = new Global();
			typeof(Global)
				.GetMethod("Application_Start", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.NonPublic)
				.Invoke(global, new object[]{ null, new EventArgs() });
		}

		#endregion

        #region How to ...

        #region Basic

        public interface IRobot
		{
			string Move(string order);
		}

		public interface IPilot
		{
			string Operate(string order);
		}

		public class Robot : IRobot
		{
			public Robot(IPilot pilot)
			{
				this.pilot = pilot;
			}

			protected IPilot pilot;

			public string Move(string order)
			{
				return pilot == null ? "not exist pilot" : pilot.Operate(order);
			}
		}

		public class Pilot : IPilot
		{
			public string Operate(string order)
			{
				return order + "ed";
			}
		}

		[Test]
		public void Basic()
		{
			IContainer container = new Container();

			container.Set(typeof(IRobot)).To(typeof(Robot));
			container.Set(typeof(IPilot)).To(typeof(Pilot));

			IRobot robot = container.Get(typeof(IRobot)) as IRobot;

			string result = robot.Move("jump");

			Assert.AreEqual("jumped", result);
		}

		#endregion

		#region Integration

		public class Global : System.Web.HttpApplication
		{
			private static IContainer container;

			protected void Application_Start(Object sender, EventArgs e)
			{
				container = new Container();

				container.Set(typeof(IRobot)).To(typeof(Robot));
				container.Set(typeof(IPilot)).To(typeof(Pilot));
			}

			public static object GetService(Type type)
			{
				return container.Get(type);
			}

			public static object GetService(Type type, ParameterList parameterList)
			{
				return container.Get(type, parameterList);
			}
		}

		[Test]
		public void Integration()
		{
			IRobot robot = Global.GetService(typeof(IRobot)) as IRobot;

			string result = robot.Move("jump");

			Assert.AreEqual("jumped", result);
		}

		#endregion

		#region Module

		public class Module : IModule
		{
			public Module(bool existPilot)
			{
				this.existPilot = existPilot;
			}

			private bool existPilot;

			public void Load(IContainer container)
			{
				container.Set(typeof(IRobot)).To(typeof(Robot));

				if (existPilot)
				{
					container.Set(typeof(IPilot)).To(typeof(Pilot));
				}
			}
		}

		[Test]
		public void LoadModules()
		{
			IContainer container = new Container();

			bool existPilot = false;
			IModule module = new Module(existPilot);

			container.LoadModules(module);

			IRobot robot = container.Get(typeof(IRobot)) as IRobot;

			string result = robot.Move("jump");

			Assert.AreEqual("not exist pilot", result);
		}

		#endregion

		#region Parameter

		public class NamedRobot : Robot
		{
			public NamedRobot(IPilot pilot, string name)
				: base(pilot)
			{
				this.name = name;
			}

			private string name;

			public string Name
			{
				get { return name; }
			}
		}

		[Test]
		public void Parameter()
		{
			IContainer container = new Container();

			container.Set(typeof(IRobot)).To(typeof(NamedRobot));
			container.Set(typeof(IPilot)).To(typeof(Pilot));

			ParameterList parameterList = new ParameterList();
			parameterList[typeof(IRobot), "name"] = "samurai robot";

			NamedRobot robot = container.Get(typeof(IRobot), parameterList) as NamedRobot;

			string name = robot.Name;

			Assert.AreEqual("samurai robot", name);
		}

		#endregion

		#region Remove

		[Test]
		public void Remove()
		{
			IContainer container = new Container();

			container.Set(typeof(IRobot)).To(typeof(Robot));
			container.Set(typeof(IPilot)).To(typeof(Pilot));

			IRobot robot = container.Get(typeof(IRobot)) as IRobot;

			Assert.AreEqual(false, robot == null);

			container.Set(typeof(IRobot)).Remove();

			robot = container.Get(typeof(IRobot)) as IRobot;

			Assert.AreEqual(true, robot == null);
		}

		#endregion

		#region Callin

		private object CreateRobot(Type serviceType, IContainer container, ParameterList parameterList)
		{
			IPilot pilot = new Pilot();
			return new Robot(pilot);
		}

		[Test]
		public void Callin()
		{			
			IContainer container = new Container();

			container.Set(typeof(IRobot)).To(typeof(Robot)).Callin(new CreationFunc(CreateRobot));

			IRobot robot = container.Get(typeof(IRobot)) as IRobot;

			string result = robot.Move("jump");

			Assert.AreEqual("jumped", result);
		}

		#endregion

		#region Callback

		public class AttackRobot : Robot
		{
			public AttackRobot(IPilot pilot) : base(pilot) { }

			private string weapon;

			public void SetWeapon(string weapon)
			{
				this.weapon = weapon;
			}

			public string Attack()
			{
				return "attack with the " + weapon;
			}
		}

		private void PostCreateRobot(Type serviceType, object instance, IContainer container, ParameterList parameterList)
		{
			AttackRobot robot = instance as AttackRobot;
			robot.SetWeapon("drill punch");
		}

		[Test]
		public void Callback()
		{	
			IContainer container = new Container();

			container.Set(typeof(IRobot)).To(typeof(AttackRobot)).Callback(new PostFunc(PostCreateRobot));

			AttackRobot robot = container.Get(typeof(IRobot)) as AttackRobot;

			string result = robot.Attack();

			Assert.AreEqual("attack with the drill punch", result);
		}		

		#endregion

		#region Singleton

		[Test]
		public void Singleton()
		{
			IContainer container = new Container();

			container.Set(typeof(IRobot)).To(typeof(AttackRobot)).Singleton();
			container.Set(typeof(IPilot)).To(typeof(Pilot));

			AttackRobot robot = container.Get(typeof(IRobot)) as AttackRobot;
			robot.SetWeapon("drill punch");

			string result = robot.Attack();

			Assert.AreEqual("attack with the drill punch", result);

			AttackRobot robot2 = container.Get(typeof(IRobot)) as AttackRobot;

			result = robot2.Attack();

			Assert.AreEqual("attack with the drill punch", result);

			Assert.AreEqual(true, robot.Equals(robot2));
		}

		#endregion

		#region PropertiesAutowired

		public interface IFlight
		{
			string Do();
		}

		public class Flight : IFlight
		{
			public string Do()
			{
				return "altitude 1000m";
			}
		}

		public class FlightRobot : Robot
		{
			public FlightRobot(IPilot pilot) : base(pilot) { }

			private IFlight flight;

			public IFlight Flight
			{
				set { flight = value; }
			}

			public string Fly()
			{
				return flight.Do();
			}
		}


		[Test]
		public void PropertiesAutowired()
		{
			IContainer container = new Container();

			container.Set(typeof(IRobot)).To(typeof(FlightRobot)).PropertiesAutowired();
			container.Set(typeof(IPilot)).To(typeof(Pilot));
			container.Set(typeof(IFlight)).To(typeof(Flight));

			FlightRobot robot = container.Get(typeof(IRobot)) as FlightRobot;

			string result = robot.Fly();

			Assert.AreEqual("altitude 1000m", result);
		}

		#endregion

		#region UsingConstructor

		public interface IDiving
		{
			string Do();
		}

		public class Diving : IDiving
		{
			public string Do()
			{
				return "depth 1000m";
			}
		}

		public class DivingRobot : Robot
		{
			public DivingRobot(IDiving diving) : base(null)
			{
				this.diving = diving;
			}

			public DivingRobot(IPilot pilot, IDiving diving)
				: base(pilot) 
			{
				this.diving = diving; 
			}

			private IDiving diving;

			public string Dive()
			{
				return diving.Do();
			}

			public void SetPilot(IPilot pilot)
			{
				this.pilot = pilot;
			}
		}

		public void UsingConstructor()
		{
			IContainer container = new Container();

			container.Set(typeof(IRobot)).To(typeof(DivingRobot)).UsingConstructor(typeof(IDiving));
			container.Set(typeof(IPilot)).To(typeof(Pilot));
			container.Set(typeof(IDiving)).To(typeof(Diving));            

			DivingRobot robot = container.Get(typeof(IRobot)) as DivingRobot;

			string result = robot.Move("jump");

			Assert.AreEqual("not exist pilot", result);

			IPilot pilot = new Pilot();
			robot.SetPilot(pilot);

			result = robot.Move("jump");

			Assert.AreEqual("jumped", result);

			result = robot.Dive();

			Assert.AreEqual("depth 1000m", result);            
		}

		#endregion

        #endregion
    }
}
