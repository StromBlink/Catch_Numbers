using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

namespace Voodoo.Utils
{
    public static class ObjectExtension
    {
        //public static object DeepCopy(this object obj)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        IFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(ms, obj);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        return formatter.Deserialize(ms);
        //    }
        //}

        public static object DeepCopy(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            Type type = obj.GetType();

            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }

            if (type.IsArray)
            {
                Type elementType = Type.GetType(type.FullName.Replace("[]", string.Empty));
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(DeepCopy(array.GetValue(i)), i);
                }

                return Convert.ChangeType(copied, type);
            }

            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                IDictionary dictionary = obj as IDictionary;
                
                object copy = FormatterServices.GetUninitializedObject(type);
                IDictionary copiedDictionary = copy as IDictionary;

                IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    copiedDictionary.Add(DeepCopy(enumerator.Key), DeepCopy(enumerator.Value));
                }

                return Convert.ChangeType(copiedDictionary, type);
            }

            if (type.IsClass)
            {
                object copy = FormatterServices.GetUninitializedObject(type);
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    object fieldValue = field.GetValue(obj);
                    if (fieldValue == null)
                    {
                        continue;
                    }

                    field.SetValue(copy, DeepCopy(fieldValue));
                }

                return copy;
            }
            
            throw new ArgumentException("Unknown type");
        }
    }
} 