TinySyringe
===========

Dependency Injection Library

## Getting started
```cs
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
```
```cs
IContainer container = new Container();

container.Set(typeof(IRobot)).To(typeof(Robot));
container.Set(typeof(IPilot)).To(typeof(Pilot));

IRobot robot = container.Get(typeof(IRobot)) as IRobot;

string result = robot.Move("jump"); // result is "jumped"
```
