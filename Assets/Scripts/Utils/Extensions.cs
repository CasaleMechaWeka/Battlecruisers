using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils
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
            Transform namedTransform = transform.Find(componentName);
            Assert.IsNotNull(namedTransform);

            T namedComponent = namedTransform.gameObject.GetComponent<T>();
            Assert.IsNotNull(namedComponent);

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

        // FELIX  Remove?
        public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> itemsToEnqueue)
        {
            foreach (T itemToEnqueue in itemsToEnqueue)
            {
                queue.Enqueue(itemToEnqueue);
			}
        }
	}
}
