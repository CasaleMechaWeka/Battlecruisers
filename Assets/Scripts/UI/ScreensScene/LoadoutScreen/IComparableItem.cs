using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public interface IComparableItem
	{
		Sprite Sprite { get; }
        string Description { get; }
        string Name { get; }
	}
}
