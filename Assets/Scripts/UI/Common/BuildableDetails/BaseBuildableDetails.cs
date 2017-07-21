using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public abstract class BaseBuildableDetails<TItem> : MonoBehaviour, IComparableItemDetails<TItem> where TItem : Buildable
	{
		private ISpriteFetcher _spriteFetcher;
		protected TItem _buildable;

		public BuildableStatsController statsController;
		public Text buildableName;
		public Text buildableDescription;
		public Image buildableImage;
		public Image slotImage;

		public void Initialise(ISpriteFetcher spriteFetcher)
		{
			_spriteFetcher = spriteFetcher;
			Hide();
		}

		public void ShowItemDetails(TItem buildable, TItem buildableToCompareTo = null)
		{
			Assert.IsNotNull(buildable);

			if (_buildable != null)
			{
				CleanUp();
			}

			_buildable = buildable;
			gameObject.SetActive(true);

			statsController.ShowStats(_buildable, buildableToCompareTo);
			buildableName.text = _buildable.buildableName;
			buildableDescription.text = _buildable.description;
			buildableImage.sprite = _buildable.Sprite;

			bool hasSlot = _buildable.slotType != SlotType.None;
			if (hasSlot)
			{
				slotImage.sprite = _spriteFetcher.GetSlotSprite((SlotType)_buildable.slotType);
			}
			slotImage.gameObject.SetActive(hasSlot);
		}

		public void Hide()
		{
			CleanUp();
			gameObject.SetActive(false);
		}

		protected virtual void CleanUp() { }
	}
}
