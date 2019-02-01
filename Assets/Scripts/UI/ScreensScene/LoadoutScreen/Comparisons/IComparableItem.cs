using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons
{
	public interface IComparableItem
	{
		Sprite Sprite { get; }
        string Description { get; }
        string Name { get; }
	}
}
