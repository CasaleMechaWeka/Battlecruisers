using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityCommon.Utils
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

		public static T GetComponentInInactiveParent<T>(this GameObject gameObject)
		{
			T[] componentAsList = gameObject.GetComponentsInParent<T>(includeInactive: true);
			Assert.IsTrue(componentAsList.Length == 1);
			return componentAsList[0];
		}

        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy(item => UnityEngine.Random.value);
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

        public static bool SmartEquals(this object original, object other)
        {
            return
                ReferenceEquals(original, other)
                || (original != null && original.Equals(other));
        }

        public static int Remove<TItem>(this ObservableCollection<TItem> items, Func<TItem, bool> condition)
        {
            IList<TItem> itemsToRemove = items.Where(condition).ToList();

            foreach (TItem itemToRemove in itemsToRemove)
            {
                items.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }
    }
}
