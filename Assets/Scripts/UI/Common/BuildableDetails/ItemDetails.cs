using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public abstract class ItemDetails<TItem> : MonoBehaviour, IComparableItemDetails<TItem>, IHidable
        where TItem : class, ITarget, IComparableItem
    {
        public Text itemName, itemDescription;
        public Image itemImage;
        private StatsController<TItem> _statsController;

        protected TItem _item;

        public event EventHandler Dismissed;

        public virtual void Initialise()
        {
            Helper.AssertIsNotNull(itemName, itemDescription, itemImage);

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

            _statsController.ShowStatsOfVariant(item,variant, itemToCompareTo);
            itemName.text = item.Name;
            itemDescription.text = item.Description;
            itemImage.sprite = item.Sprite;

            gameObject.SetActive(true);
        }

        public virtual void ShowItemDetails()
        {
            _statsController.ShowStats(_item);
            itemName.text = _item.Name;
            itemDescription.text = _item.Description;
            itemImage.sprite = _item.Sprite;
        }

        public virtual void SetHullType(HullType hullType)
        {
            if (GetComponent<BodykitDetailController>() != null)
            {
                GetComponent<BodykitDetailController>().hullType = hullType;
            }
        }

        public virtual void SetBuilding(IBuilding building)
        {
            if (GetComponent<BuildingDetailController>() != null)
            {
                GetComponent<BuildingDetailController>().SelectedBuilding = building;
            }
        }

        public virtual void SetBuilding(IBuilding building, ItemButton button)
        {
            if (GetComponent<BuildingDetailController>() != null)
            {
                GetComponent<BuildingDetailController>().CureentButton = button;
                GetComponent<BuildingDetailController>().SelectedBuilding = building;
            }
        }
        public virtual void SetUnit(IUnit unit)
        {
            if (GetComponent<UnitDetailController>() != null)
            {
                GetComponent<UnitDetailController>().SelectedUnit = unit;
            }
        }

        public virtual void SetUnit(IUnit unit, ItemButton button)
        {
            if (GetComponent<UnitDetailController>() != null)
            {
                GetComponent<UnitDetailController>().currentButton = button;
                GetComponent<UnitDetailController>().SelectedUnit = unit;
            }
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
