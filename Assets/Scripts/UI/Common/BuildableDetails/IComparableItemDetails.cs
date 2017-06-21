using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	public interface IComparableItemDetails<T> where T : Target
	{
		void ShowItemDetails(T item, T itemToCompareTo = null);
	}
}

