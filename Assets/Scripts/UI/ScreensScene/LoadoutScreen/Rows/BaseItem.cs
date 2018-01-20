using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public abstract class BaseItem<TItem> : MonoBehaviour, IItem<TItem> where TItem : IComparableItem
	{
		public static class Colors
		{
			public readonly static Color ENABLED = Color.green;
			public readonly static Color DEFAULT = Color.clear;
		}

		public Image itemImage;
		public Image backgroundImage;
		public Image selectedFeedbackImage;

		public abstract TItem Item { get; protected set; }

		public bool ShowSelectedFeedback
		{
			set { selectedFeedbackImage.gameObject.SetActive(value); }
		}
	}
}
