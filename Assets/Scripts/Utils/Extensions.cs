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
	}
}
