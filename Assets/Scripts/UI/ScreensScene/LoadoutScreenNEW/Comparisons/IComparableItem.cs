using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons
{
	public interface IComparableItem
	{
		Sprite Sprite { get; }
        string Description { get; }
        string Name { get; }
	}
}
