using System;
using System.Collections.Generic;

namespace Voodoo.Utils
{
    public static class TypeExtension
    {
        /// <summary>
        /// Returns the IEnumerable element type.
        /// Example: List of string will return string
        /// </summary>
        /// <param name="seqType"></param>
        /// <returns></returns>
        public static Type GetIEnumerableElementType(this Type seqType)
        {
            Type ienum = seqType.FindIEnumerable();
            if (ienum == null)
            {
                return seqType;
            }

            return ienum.GetGenericArguments()[0];
        }
        
        /// <summary>
        /// Returns all the types that derives from type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Type> GetAllDerivedTypes(this Type type)
        {
            return TypeUtility.GetTypes(curType => curType.IsSubclassOf(type));
        }
        
        /// <summary>
        /// Returns all the types that implements the interface interfaceType 
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static List<Type> GetTypesFromInterface(this Type interfaceType)
        {
            return TypeUtility.GetTypes(assembly => true, type => interfaceType.IsAssignableFrom(type) && type.IsAbstract == false);
        }

        /// <summary>
        /// Returns the type friendlyName
        /// Example: The friendly name for CollectibleProgress`1[[CollectibleConfig]] is CollectibleProgress[CollectibleConfig]
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string FriendlyName(this Type type)
        {
            return type.SimplifyName('[', ']', false);
        }

        /// <summary>
        /// Returns the type genericName. Same as FriendlyName but using chevrons
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GenericName(this Type type)
        {
            return type.SimplifyName('<', '>', true);
        }

        private static string SimplifyName(this Type type, char startingChar, char endingChar, bool useFullName)
        {
            string simpleName = useFullName ? type.FullName : type.Name;
            if (type.IsGenericType)
            {
                int backtickIndex = simpleName.IndexOf('`');
                if (backtickIndex > 0)
                {
                    simpleName = simpleName.Remove(backtickIndex);
                }

                simpleName += startingChar;

                Type[] typeParameters = type.GetGenericArguments();
                for (int i = 0; i < typeParameters.Length; ++i)
                {
                    string typeParamName = SimplifyName(typeParameters[i], startingChar, endingChar, useFullName);
                    simpleName += (i == 0 ? typeParamName : "," + typeParamName);
                }

                simpleName += endingChar;
            }

            return simpleName;
        }

        public static object CreateInstance(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) == null ? System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type) : Activator.CreateInstance(type);
        }
        
        private static Type FindIEnumerable(this Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
            {
                return null;
            }

            if (seqType.IsArray)
            {
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
            }

            if (seqType.IsGenericType)
            {
                foreach (Type arg in seqType.GetGenericArguments())
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                    {
                        return ienum;
                    }
                }
            }

            Type[] ifaces = seqType.GetInterfaces();
            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (Type iface in ifaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null) return ienum;
                }
            }

            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
            {
                return FindIEnumerable(seqType.BaseType);
            }

            return null;
        }
    }
}
