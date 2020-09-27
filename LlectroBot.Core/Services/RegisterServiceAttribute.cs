using System;

namespace LlectroBot.Core.Services
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RegisterServiceAttribute : Attribute
    {
        public Type Interface { get; set; }
        public bool IsValid { get; private set; }
        public RegisterServiceAttribute(Type _interface)
        {
            IsValid = _interface.IsInterface;
            Interface = _interface;
        }
    }
}
