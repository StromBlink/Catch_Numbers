using System;
using System.Collections.Generic;
using System.Reflection;

namespace Voodoo.Utils
{
	public class TypeUtility
	{
        public const string assemblyCSharp = "Assembly-CSharp";
        private const string assemblyCSharpFirstPass = "Assembly-CSharp-firstpass";
        private const string assemblyCSharpEditor = "Assembly-CSharp-Editor";
        private const string assemblyCSharpEditorFirstPass = "Assembly-CSharp-Editor-firstpass";

        private static Assembly[] assemblies;
        private static Assembly[] Assemblies
        {
            get
            {
                if (assemblies != null)
                {
                    return assemblies;
                }

                return assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }
            set => assemblies = value;
        }
     
        /* TODO handle functions param in a better way
         * for instance assembly are only considered for their name so 
         *  - why not having an enum
         *  - or just have predicate on type for the main / editor / plugins and so on functions
        */
        /// <summary>
        /// Returns all the types that matches the condition typeCondition
        /// </summary>
        /// <param name="typeCondition"></param>
        /// <returns></returns>
        public static List<Type> GetTypes(Predicate<Type> typeCondition)
        {
            return GetTypes(assembly => true, typeCondition);
        }
        
        /// <summary>
        /// Returns all the types that matches both conditions assemblyCondition and typeCondition
        /// </summary>
        /// <param name="assemblyCondition"></param>
        /// <param name="typeCondition"></param>
        /// <returns></returns>
        public static List<Type> GetTypes(Predicate<Assembly> assemblyCondition, Predicate<Type> typeCondition)
        {
            List<Type> allTypes = GetTypes(assemblyCondition);
            
            List<Type> result = new List<Type>();
            foreach (Type type in allTypes)
            {
                if (typeCondition(type))
                {
                    result.Add(type);
                }
            }
            return result;
        }
        
        /// <summary>
        /// Returns all the types that matches the condition assemblyCondition
        /// </summary>
        /// <param name="assemblyCondition"></param>
        /// <returns></returns>
        public static List<Type> GetTypes(Predicate<Assembly> assemblyCondition)
        {
            List<Type> alltypes = new List<Type>();
            foreach (Assembly assembly in Assemblies)
            {
                if (assemblyCondition(assembly))
                {
                    alltypes.AddRange(assembly.GetTypes());
                }
            }

            return alltypes;
        }

        /// <summary>
        /// Returns all the exported types that matches the condition typeCondition
        /// </summary>
        /// <param name="typeCondition"></param>
        /// <returns></returns>
        public static List<Type> GetExportedTypes(Predicate<Type> typeCondition)
        {
            return GetExportedTypes(assembly => true, typeCondition);
        }
        
        /// <summary>
        /// Returns all the exported types that matches both conditions assemblyCondition and typeCondition
        /// </summary>
        /// <param name="assemblyCondition"></param>
        /// <param name="typeCondition"></param>
        /// <returns></returns>
        public static List<Type> GetExportedTypes(Predicate<Assembly> assemblyCondition, Predicate<Type> typeCondition)
        {
            List<Type> allTypes = GetTypes(assemblyCondition);
            
            List<Type> result = new List<Type>();
            foreach (Type type in allTypes)
            {
                if (typeCondition(type))
                {
                    result.Add(type);
                }
            }
            return result;
        }
        
        /// <summary>
        /// Returns all the exported types that matches the condition assemblyCondition
        /// </summary>
        /// <param name="assemblyCondition"></param>
        /// <returns></returns>
        public static List<Type> GetExportedTypes(Predicate<Assembly> assemblyCondition)
        {
            List<Type> result = new List<Type>();
            foreach (Assembly assembly in Assemblies)
            {
                if (assemblyCondition(assembly))
                {
                    result.AddRange(assembly.ExportedTypes);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the types that can be found in the Assembly-CSharp-firstpass
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetPluginTypes()
        {
            return GetTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharpFirstPass));
        }
        
        /// <summary>
        /// Returns the types that can be found in the Assembly-CSharp-Editor-firstpass
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetEditorPluginTypes()
        {
            return GetTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharpEditorFirstPass));
        }

        /// <summary>
        /// Returns the types that can be found in the Assembly-CSharp
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetMainTypes()
        {
            return GetTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharp));
        }

        /// <summary>
        /// Returns the types that can be found in the Assembly-CSharp-Editor
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetEditorTypes()
        {
            return GetTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharpEditor));
        }
        
        /// <summary>
        /// Returns the exported types that can be found in the Assembly-CSharp-firstpass
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetPluginExportedTypes()
        {
            return GetExportedTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharpFirstPass));
        }
        
        /// <summary>
        /// Returns the exported types that can be found in the Assembly-CSharp-Editor-firstpass
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetEditorPluginExportedTypes()
        {
            return GetExportedTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharpEditorFirstPass));
        }
        
        /// <summary>
        /// Returns the exported types that can be found in the Assembly-CSharp
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetMainExportedTypes()
        {
            return GetExportedTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharp));
        }
        
        /// <summary>
        /// Returns the exported types that can be found in the Assembly-CSharp-Editor
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetEditorExportedTypes()
        {
            return GetExportedTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharpEditor));
        }
        
        /// <summary>
        /// Returns all the types of your project
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetAllTypes()
        {
            return GetTypes(assembly => true , type => true);
        }
        
        /// <summary>
        /// Returns all the exported types of your project
        /// The exported types are the types that are visible outside of your assemblies
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetAllExportedTypes()
        {
            return GetExportedTypes(assembly => true , type => true);
        }

        /// <summary>
        /// Returns the type which FullName matches your fullname
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static Type GetType(string fullname)
        {
            List<Type> types = GetTypes((Type curType) => curType.FullName == fullname);
            return types.Count > 0 ? types[0] : null;
        }

        /// <summary>
        /// Returns the exported type which FullName matches your fullname
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static Type GetExportedType(string fullname)
        {
            List<Type> types = GetExportedTypes((Type curType) => curType.FullName == fullname);
            return types.Count > 0 ? types[0] : null;
        }

        /// <summary>
        /// Returns all the types that can be found in the following assemblies:
        /// Assembly-CSharp, Assembly-CSharp-Editor
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetMainpassTypes()
        {
            return GetTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharp) || assembly.FullName.Contains(assemblyCSharpEditor));
        }

        /// <summary>
        /// Returns all the types that can be found in the following assemblies:
        /// Assembly-CSharp-firstpass, Assembly-CSharp-Editor-firstpass
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetFirstspassTypes()
        {
            return GetTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharpFirstPass) || assembly.FullName.Contains(assemblyCSharpEditorFirstPass));
        }

        /// <summary>
        /// Returns all the exported types that can be found in the following assemblies:
        /// Assembly-CSharp, Assembly-CSharp-Editor
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetMainpassExportedTypes()
        {
            return GetExportedTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharp) || assembly.FullName.Contains(assemblyCSharpEditor));
        }
        
        /// <summary>
        /// Returns all the exported types that can be found in the following assemblies:
        /// Assembly-CSharp-firstpass, Assembly-CSharp-Editor-firstpass
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetFirstspassExportedTypes()
        {
            return GetExportedTypes((Assembly assembly) => assembly.FullName.Contains(assemblyCSharpFirstPass) || assembly.FullName.Contains(assemblyCSharpEditorFirstPass));
        }
	}
}