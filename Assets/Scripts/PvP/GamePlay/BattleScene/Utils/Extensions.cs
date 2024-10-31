using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils
{
    public static class Extensions
    {
        public static int GetHashCode(this object obj, params object[] fields)
        {
            int hash = 17;

            foreach (object field in fields)
            {
                if (field != null)
                {
                    hash *= 23 * field.GetHashCode();
                }
            }

            return hash;
        }

        public static bool IsMirrored(this Transform transform)
        {
            return transform.rotation.eulerAngles.y == 180;
        }

        public static T GetComponentInInactiveParent<T>(this GameObject gameObject)
        {
            T[] componentAsList = gameObject.GetComponentsInParent<T>(includeInactive: true);
            Assert.IsTrue(componentAsList.Length == 1);
            return componentAsList[0];
        }

        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy(item => Random.value);
        }

        public static T Parse<T>(this object objToParse) where T : class
        {
            T parsedObj = objToParse as T;
            Assert.IsNotNull(parsedObj);
            return parsedObj;
        }

        public static T FindNamedComponent<T>(this Transform transform, string componentName)
            where T : class
        {
            if (transform == null)
            {
                Debug.LogError("Transform is null.");
                return null;
            }

            Debug.Log($"Searching for component '{typeof(T).Name}' in '{transform.name}' with component name '{componentName}'");

            Transform namedTransform = transform.Find(componentName);
            if (namedTransform == null)
            {
                Debug.LogError($"Transform with name '{componentName}' not found in '{transform.name}'");
                return null; // Return null or handle the error as needed
            }

            T namedComponent = namedTransform.gameObject.GetComponent<T>();
            if (namedComponent == null)
            {
                Debug.LogError($"Component '{typeof(T).Name}' not found on '{namedTransform.name}'");
                return null; // Return null or handle the error as needed
            }

            return namedComponent;
        }

        public static T Middle<T>(this IList<T> list)
        {
            Assert.IsTrue(list.Count != 0);
            return list[list.Count / 2];
        }

        public static string GetFileName(this string path, string deliminator = "/")
        {
            int index = path.LastIndexOf(deliminator);

            return
                index == -1 || index == path.Length - 1 ?
                path :
                path.Substring(index + 1);
        }

        public static bool SmartEquals(this object original, object other)
        {
            return
                ReferenceEquals(original, other)
                || (original != null && original.Equals(other));
        }
    }
}
