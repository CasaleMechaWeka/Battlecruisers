using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Properties;
using System.Diagnostics;
using static BattleCruisers.Effects.Smoke.StaticSmokeStats;

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
            IDataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
            int index = await dataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_prefabFactory, building);
            if (index != -1)
            {
                VariantPrefab variant = await _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                IBuilding staticBuilding = variant.GetBuilding(_prefabFactory);
                _buildingDetails.ShowItemDetails(staticBuilding, variant);
                _buildingDetails.GetBuildingVariantDetailController().variantName.text = _commonString.GetString(dataProvider.GameModel.Variants[index].variantNameStringKeyBase) + " " + _commonString.GetString("Buildables/Buildings/" + building.keyName + "Name");
                //_buildingDetails.GetBuildingVariantDetailController().variantDescription.text = _commonString.GetString(dataProvider.GameModel.Variants[index].variantDescriptionStringKeyBase);
                _buildingDetails.GetBuildingVariantDetailController().variantIcon.gameObject.SetActive(true);
                _buildingDetails.GetBuildingVariantDetailController().variantIcon.sprite = variant.variantSprite;
                _selectedItem.Value = building;
            }
            else
            {
                _buildingDetails.GetBuildingVariantDetailController().variantIcon.gameObject.SetActive(false);
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
            IDataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
            int index = await dataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(_prefabFactory, unit);
            if (index != -1)
            {
                VariantPrefab variant = await _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                IUnit staticUnit = variant.GetUnit(_prefabFactory);
                _unitDetails.ShowItemDetails(staticUnit, variant);
                _unitDetails.GetUnitVariantDetailController().variantName.text = _commonString.GetString(dataProvider.GameModel.Variants[index].variantNameStringKeyBase) + " " + _commonString.GetString("Buildables/Units/" + unit.keyName + "Name");
                //_unitDetails.GetUnitVariantDetailController().variantDescription.text = _commonString.GetString(dataProvider.GameModel.Variants[index].variantDescriptionStringKeyBase);
                _unitDetails.GetUnitVariantDetailController().variantIcon.gameObject.SetActive(true);
                _unitDetails.GetUnitVariantDetailController().variantIcon.sprite = variant.variantSprite;
                _selectedItem.Value = unit;
            }
            else
            {
                _unitDetails.GetUnitVariantDetailController().variantIcon.gameObject.SetActive(false);
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
