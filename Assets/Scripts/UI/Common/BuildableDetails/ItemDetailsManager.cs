using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Properties;
using System.Diagnostics;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class ItemDetailsManager : IItemDetailsManager
    {
        private readonly IInformatorPanel _informatorPanel;
        private readonly IComparableItemDetails<IBuilding> _buildingDetails;
        private readonly IComparableItemDetails<IUnit> _unitDetails;
        private readonly IComparableItemDetails<ICruiser> _cruiserDetails;
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDataProvider _dataProvider;
        private readonly ILocTable _commonString;

        private ISettableBroadcastingProperty<ITarget> _selectedItem;
        public IBroadcastingProperty<ITarget> SelectedItem { get; }

        public ItemDetailsManager(IInformatorPanel informator, IDataProvider dataProvider, IPrefabFactory prefabFactory, ILocTable commonString)
        {
            Helper.AssertIsNotNull(informator, dataProvider, prefabFactory, commonString);

            _informatorPanel = informator;
            _buildingDetails = informator.BuildingDetails;
            _unitDetails = informator.UnitDetails;
            _cruiserDetails = informator.CruiserDetails;
            _commonString = commonString;

            _prefabFactory = prefabFactory;
            _dataProvider = dataProvider;

            _selectedItem = new SettableBroadcastingProperty<ITarget>(initialValue: null);
            SelectedItem = new BroadcastingProperty<ITarget>(_selectedItem);
            _dataProvider = dataProvider;
        }

        public void ShowDetails(IBuilding building)
        {
            HideInformatorContent();

            _informatorPanel.Show(building);
            ShowItemDetailsV2(building);
            /*            _buildingDetails.ShowItemDetails(building);
                        _selectedItem.Value = building;*/
        }

        private async void ShowItemDetailsV2(IBuilding building)
        {
            VariantPrefab variant = await _dataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariant(_prefabFactory, building);
            if (variant != null)
            {
                int index = await _dataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_prefabFactory, building);
                building.OverwriteComparableItem(_commonString.GetString(_dataProvider.GameModel.Variants[index].VariantNameStringKeyBase), _commonString.GetString(_dataProvider.GameModel.Variants[index].variantDescriptionStringKeyBase));
                _buildingDetails.ShowItemDetails(building, variant);
                _selectedItem.Value = building;
            }
            else
            {
                _buildingDetails.ShowItemDetails(building);
                _selectedItem.Value = building;
            }
        }

        public void SelectBuilding(IBuilding building)
        {
            _selectedItem.Value = building;
        }

        public void ShowDetails(IUnit unit)
        {
            HideInformatorContent();

            _informatorPanel.Show(unit);
            ShowItemDetailsV2(unit);
/*            _unitDetails.ShowItemDetails(unit);
            _selectedItem.Value = unit;*/
        }

        private async void ShowItemDetailsV2(IUnit unit)
        {
            VariantPrefab variant = await _dataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariant(_prefabFactory, unit);
            if (variant != null)
            {
                UnityEngine.Debug.Log("===> CCC");
                int index = await _dataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(_prefabFactory, unit);
                unit.OverwriteComparableItem(_commonString.GetString(_dataProvider.GameModel.Variants[index].VariantNameStringKeyBase),_commonString.GetString(_dataProvider.GameModel.Variants[index].variantDescriptionStringKeyBase));
                _unitDetails.ShowItemDetails(unit, variant);
                _selectedItem.Value = unit;
            }
            else
            {
                UnityEngine.Debug.Log("===> DDD");
                _unitDetails.ShowItemDetails(unit);
                _selectedItem.Value = unit;
            }
        }

        public void SelectUnit(IUnit unit)
        {
            _selectedItem.Value = unit;
        }

        public void ShowDetails(ICruiser cruiser)
        {
            HideInformatorContent();

            _informatorPanel.Show(cruiser);
            _cruiserDetails.ShowItemDetails(cruiser);
            _selectedItem.Value = cruiser;
        }

        public void HideDetails()
        {
            _informatorPanel.Hide();
            _selectedItem.Value = null;
        }

        private void HideInformatorContent()
        {
            _buildingDetails.Hide();
            _unitDetails.Hide();
            _cruiserDetails.Hide();
        }
    }
}
