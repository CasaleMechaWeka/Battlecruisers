using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public class PvPItemDetailsManager : IPvPItemDetailsManager
    {
        private readonly IPvPInformatorPanel _informatorPanel;
        private readonly IPvPComparableItemDetails<IPvPBuilding> _buildingDetails;
        private readonly IPvPComparableItemDetails<IPvPUnit> _unitDetails;
        private readonly IPvPComparableItemDetails<IPvPCruiser> _cruiserDetails;

        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IDataProvider _dataProvider;
        private readonly ILocTable _commonString;

        private IPvPSettableBroadcastingProperty<ITarget> _selectedItem;
        public IBroadcastingProperty<ITarget> SelectedItem { get; }

        public PvPItemDetailsManager(IPvPInformatorPanel informator, IDataProvider dataProvider, IPvPPrefabFactory prefabFactory, ILocTable commonString)
        {
            PvPHelper.AssertIsNotNull(informator, dataProvider, prefabFactory, commonString);

            _informatorPanel = informator;
            _buildingDetails = informator.BuildingDetails;
            _unitDetails = informator.UnitDetails;
            _cruiserDetails = informator.CruiserDetails;

            _commonString = commonString;
            _prefabFactory = prefabFactory;
            _dataProvider = dataProvider;

            _selectedItem = new PvPSettableBroadcastingProperty<ITarget>(initialValue: null);
            SelectedItem = new PvPBroadcastingProperty<ITarget>(_selectedItem);
        }

        public void ShowDetails(IPvPBuilding building)
        {
            HideInformatorContent();

            _informatorPanel.Show(building);
            ShowItemDetailsV2(building);
            /*            _buildingDetails.ShowItemDetails(building);
                        _selectedItem.Value = building;*/
        }

        private async void ShowItemDetailsV2(IPvPBuilding building)
        {
            int index = await _dataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_prefabFactory, building);

            if (index != -1)
            {
                VariantPrefab variant = await _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                IPvPBuilding staticBuilding = variant.GetBuilding(_prefabFactory);
                _buildingDetails.ShowItemDetails(staticBuilding, variant);
                _buildingDetails.GetBuildingVariantDetailController().variantName.text = _commonString.GetString(_dataProvider.StaticData.Variants[index].VariantNameStringKeyBase) + " " + _commonString.GetString("Buildables/Buildings/" + building.keyName + "Name");
                //_buildingDetails.GetBuildingVariantDetailController().variantDescription.text = _commonString.GetString(_dataProvider.GameModel.Variants[index].variantDescriptionStringKeyBase);
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

        public void SelectBuilding(IPvPBuilding building)
        {
            _selectedItem.Value = building;
        }

        public void ShowDetails(IPvPUnit unit)
        {
            HideInformatorContent();

            _informatorPanel.Show(unit);
            ShowItemDetailsV2(unit);
            /*            _unitDetails.ShowItemDetails(unit);
                        _selectedItem.Value = unit;*/
        }

        private async void ShowItemDetailsV2(IPvPUnit unit)
        {
            int index = await _dataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(_prefabFactory, unit);
            if (index != -1)
            {
                VariantPrefab variant = await _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                IPvPUnit staticUnit = variant.GetUnit(_prefabFactory);
                _unitDetails.ShowItemDetails(staticUnit, variant);
                _unitDetails.GetUnitVariantDetailController().variantName.text = _commonString.GetString(_dataProvider.StaticData.Variants[index].VariantNameStringKeyBase) + " " + _commonString.GetString("Buildables/Units/" + unit.keyName + "Name");
                //_unitDetails.GetUnitVariantDetailController().variantDescription.text = _commonString.GetString(_dataProvider.GameModel.Variants[index].variantDescriptionStringKeyBase);
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

        public void SelectUnit(IPvPUnit unit)
        {
            _selectedItem.Value = unit;
        }

        public void ShowDetails(IPvPCruiser cruiser)
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
