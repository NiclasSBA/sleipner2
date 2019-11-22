using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sleipner.Core.Netcore
{
    public class SleipnerGenericParameterMustBeReferenceException : Exception
    {
        public SleipnerGenericParameterMustBeReferenceException(MethodInfo method, Type t) : base("You must constraint method " + method.Name + " with generic parameter " + t.Name + " to be a reference type only (where " + t.Name + " : class)")
        {

        }
    }
}
