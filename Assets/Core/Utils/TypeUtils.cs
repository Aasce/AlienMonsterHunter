using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Asce.Managers.Utils
{
	/// <summary>
	///     Provides utility methods for working with <see cref="Type"/> objects,
	///     specifically for retrieving concrete subclasses of a given type.
	/// </summary>
    public static class TypeUtils
    {
		/// <summary>
		///     Retrieves all non-abstract, non-interface types that inherit from or implement
		///     the specified generic type <typeparamref name="T"/> across all loaded assemblies
		///     in the current <see cref="AppDomain"/>.
		/// </summary>
		/// <typeparam name="T">The base class or interface to search for subclasses/implementations of.</typeparam>
		/// <returns>
		///     A <see cref="List{T}"/> of <see cref="Type"/> objects representing the concrete subclasses
		///     or implementations of <typeparamref name="T"/>.
		/// </returns>
        public static List<Type> GetConcreteSubclassesOf<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => SafeGetTypes(x))
                .Where(t => typeof(T).IsAssignableFrom(t)
                            && !t.IsAbstract
                            && !t.IsInterface)
                .ToList();
        }

		/// <summary>
		///     Safely retrieves all types from the specified <see cref="Assembly"/>.
		///     This method handles <see cref="ReflectionTypeLoadException"/> and filters out any null types.
		/// </summary>
		/// <param name="assembly">The assembly from which to retrieve types.</param>
		/// <returns>
		///     An <see cref="IEnumerable{T}"/> of <see cref="Type"/> objects from the specified assembly,
		///     excluding any null values.
		/// </returns>
        private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
        {
            try 
            { 
                return assembly.GetTypes(); 
            }
            catch (ReflectionTypeLoadException e) 
            { 
                return e.Types.Where(t => t != null); 
            }
        }
    }
}