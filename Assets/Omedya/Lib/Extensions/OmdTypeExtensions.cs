using System;
using System.Linq;

namespace Omedya.Lib.Extensions
{
    public static class OmdTypeExtensions
    {
        private static Type GetImplementedOpenGeneric(Type type, Type openGeneric)
        {
            return type.GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == openGeneric);
        }
        public static bool IsGenericImplemented(this Type type, Type openGeneric)
        {
            try
            {
                return GetImplementedOpenGeneric(type, openGeneric) != null;
            }
            catch
            {
                return false;
            }
        }
        public static Type GetInterfaceGenericArg(this Type type, Type interfaceType, bool checkBaseTypes,
            int argumentIndex = 0)
        {
            Type interfaceDefinition = null;

            while (type != null && type != typeof(object))
            {
                interfaceDefinition = GetImplementedOpenGeneric(type, interfaceType);

                if (!checkBaseTypes) break;

                if (interfaceDefinition == null) type = type.BaseType;
            }

            if (interfaceDefinition == null)
                return null;

            var interfaceGenericArgs = interfaceDefinition.GetGenericArguments();
            if (interfaceGenericArgs.Length <= argumentIndex)
                return null;

            return interfaceGenericArgs[argumentIndex];
        }
        
        public static bool TryGetInterfaceGenericArgs(this Type type, Type interfaceType, bool checkBaseTypes,
            out Type[] argTypes)
        {
            argTypes = null;
            Type interfaceDefinition = null;

            while (type != null && type != typeof(object))
            {
                interfaceDefinition = GetImplementedOpenGeneric(type, interfaceType);

                if (!checkBaseTypes) break;

                if (interfaceDefinition == null) type = type.BaseType;
            }

            if (interfaceDefinition == null)
                return false;

            var interfaceGenericArgs = interfaceDefinition.GetGenericArguments();

            argTypes = interfaceGenericArgs;

            return true;
        }
    }
}