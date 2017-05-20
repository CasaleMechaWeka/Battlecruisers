using System;

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
	}
}
