using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public abstract class PvPItemDetails<TItem> : MonoBehaviour
        where TItem : class, ITarget, IComparableItem
    {
        public Text itemName, itemDescription;
        public Image itemImage;
        private StatsController<TItem> _statsController;

        protected TItem _item;

        public event EventHandler Dismissed;

        public virtual void Initialise()
        {
            PvPHelper.AssertIsNotNull(itemName, itemDescription, itemImage);

            _statsController = GetStatsController();
            Assert.IsNotNull(_statsController);
            _statsController.Initialise();
        }

        protected abstract StatsController<TItem> GetStatsController();
        public virtual BuildingVariantDetailController GetBuildingVariantDetailController() { return null; }
        public virtual UnitVariantDetailController GetUnitVariantDetailController() { return null; }

        public virtual void ShowItemDetails(TItem item, TItem itemToCompareTo = default)
        {
            Assert.IsNotNull(item);

            if (_item != null)
            {
                CleanUp();
            }

            _item = item;

            _statsController.ShowStats(item, itemToCompareTo);
            itemName.text = item.Name;
            itemDescription.text = item.Description;
            itemImage.sprite = item.Sprite;

            gameObject.SetActive(true);
        }

        public virtual void ShowItemDetails(TItem item, VariantPrefab variant, TItem itemToCompareTo = default)
        {
            Assert.IsNotNull(item);

            if (_item != null)
            {
                CleanUp();
            }

            _item = item;

            _statsController.ShowStatsOfVariant(item, variant, itemToCompareTo);
            itemName.text = item.Name;
            itemDescription.text = item.Description;
            itemImage.sprite = item.Sprite;

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            CleanUp();
            gameObject.SetActive(false);

            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void CleanUp() { }
    }
}
