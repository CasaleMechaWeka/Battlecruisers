using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public abstract class BaseItem<TItem> : MonoBehaviour, 
        IStatefulUIElement,    
        IItem<TItem> where TItem : IComparableItem
	{
		public static class Colors
		{
			public readonly static Color HIGHLIGHTED = Color.green;
            public readonly static Color DEFAULT = Color.grey;
		}

		public Image itemImage;
		public Image backgroundImage;
		public Image selectedFeedbackImage;

		public abstract TItem Item { get; protected set; }

		public bool ShowSelectedFeedback
		{
			set { selectedFeedbackImage.gameObject.SetActive(value); }
		}

        public void GoToDefaultState()
        {
            throw new System.NotImplementedException();
        }

        public void GoToHighlightedState()
        {
            throw new System.NotImplementedException();
        }

        public void GoToDisabledState()
        {
            throw new System.NotImplementedException();
        }
	}
}
