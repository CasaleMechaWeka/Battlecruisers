using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.LoadoutScreen.Comparisons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public abstract class PvPItemDetails<TItem> : MonoBehaviour, IPvPComparableItemDetails<TItem>, IPvPHidable
        where TItem : class, IPvPTarget, IPvPComparableItem
    {
        public Text itemName, itemDescription;
        public Image itemImage;
        private PvPStatsController<TItem> _statsController;

        protected TItem _item;

        public event EventHandler Dismissed;

        public virtual void Initialise()
        {
            PvPHelper.AssertIsNotNull(itemName, itemDescription, itemImage);

            _statsController = GetStatsController();
            Assert.IsNotNull(_statsController);
            _statsController.Initialise();
        }

        protected abstract PvPStatsController<TItem> GetStatsController();

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

        public void Hide()
        {
            CleanUp();
            gameObject.SetActive(false);

            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void CleanUp() { }
    }
}
