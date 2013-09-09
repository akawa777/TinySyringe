using System;
using System.Data;
using System.Reflection;
using System.Collections;

namespace TinySyringe
{
    public delegate object CreationFunc(Type implementType, IContainer container, ParameterList parameterList);
    public delegate void PostFunc(Type implementType, object instance, IContainer container, ParameterList parameterList);
}