using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asce.Core.Utils
{
    public static class ComponentUtils
    {
        /// <summary>
        ///     Load component of type T if it exists in the GameObject, its children, or its parents.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <param name="result"></param>
        /// <returns> Returns true if load successful. </returns>
        public static bool LoadComponent<T>(this Component component, out T result, bool includeInactive = false) where T : Component
        {
            result = component.GetComponent<T>();
            if (result != null) return true;
            
            result = component.GetComponentInChildren<T>(includeInactive);
            if (result != null) return true;

            result = component.GetComponentInParent<T>(includeInactive);
            if (result != null) return true;

            return false;
        }


        /// <summary>
        ///     Get component of type T if it exists, otherwise add it to the GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
            return component;
        }

        /// <summary>
        ///     Get component of type T if it exists, otherwise add it to the GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            return component.GetOrAddComponent<T>();
        }

        /// <summary>
        ///     Finds all components of type T in the currently active scene, including children of root objects.
        /// </summary>
        /// <typeparam name="T"> The type of component to search for. </typeparam>
        /// <param name="includeInactive">
        ///     Whether to include components on inactive GameObjects.
        ///     Default is true.
        /// </param>
        /// <returns>
        ///     A list of all found components of type T in the active scene.
        /// </returns>
        public static List<T> FindAllComponentsInScene<T>(bool includeInactive = true)
        {
            List<T> result = new();

            foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                result.AddRange(root.GetComponentsInChildren<T>(includeInactive));
            }

            return result;
        }


        public static GameObject FindGameObjectById(string id)
        {
            IIdentifiable identifiable = ComponentUtils.FindAllComponentsInScene<IIdentifiable>().Find((iden) => iden.Id == id);
            if (identifiable != null && identifiable is Component comp)
            {
                return comp.gameObject;
            }
            return null;
        }

        public static T FindComponentById<T>(string id) where T : class, IIdentifiable
        {
            T identifiable = ComponentUtils.FindAllComponentsInScene<T>().Find((iden) => iden.Id == id);
            return identifiable;
        }

        public static string GetId(this GameObject gameObject)
        {
            if (gameObject == null) return string.Empty;
            if (gameObject.TryGetComponent(out IIdentifiable identifiable))
            {
                return identifiable.Id;
            }
            return string.Empty;
        }
    }
}