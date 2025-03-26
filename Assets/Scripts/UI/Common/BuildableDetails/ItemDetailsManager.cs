using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class ItemDetailsManager : IItemDetailsManager
    {
        private readonly IInformatorPanel _informatorPanel;
        private readonly IComparableItemDetails<IBuilding> _buildingDetails;
        private readonly IComparableItemDetails<IUnit> _unitDetails;
        private readonly IComparableItemDetails<ICruiser> _cruiserDetails;
        private readonly PrefabFactory _prefabFactory;

        private ISettableBroadcastingProperty<ITarget> _selectedItem;
        public IBroadcastingProperty<ITarget> SelectedItem { get; }

        public ItemDetailsManager(IInformatorPanel informator, PrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(informator, prefabFactory);

            _informatorPanel = informator;
            _buildingDetails = informator.BuildingDetails;
            _unitDetails = informator.UnitDetails;
            _cruiserDetails = informator.CruiserDetails;

            _prefabFactory = prefabFactory;

            _selectedItem = new SettableBroadcastingProperty<ITarget>(initialValue: null);
            SelectedItem = new BroadcastingProperty<ITarget>(_selectedItem);
        }

        public void ShowDetails(IBuilding building)
        {
            HideInformatorContent();

            _informatorPanel.Show(building);
            ShowItemDetailsV2(building);
            /*            _buildingDetails.ShowItemDetails(building);
                        _selectedItem.Value = building;*/
        }

        private void ShowItemDetailsV2(IBuilding building)
        {
            if (building.Faction == Faction.Blues)
            {
                int index = DataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_prefabFactory, building);
                if (index != -1)
                {
                    VariantPrefab variant = _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                    IBuilding staticBuilding = variant.GetBuilding(_prefabFactory);
                    _buildingDetails.ShowItemDetails(staticBuilding, variant);
                    _buildingDetails.GetBuildingVariantDetailController().variantName.text
                     = LocTableCache.CommonTable.GetString(StaticData.Variants[index].VariantNameStringKeyBase)
                     + " " + LocTableCache.CommonTable.GetString("Buildables/Buildings/" + building.keyName + "Name");
                    //_buildingDetails.GetBuildingVariantDetailController().variantDescription.text = _commonString.GetString(DataProvider.GameModel.Variants[index].variantDescriptionStringKeyBase);
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
            else
            {
                int index = building.variantIndex;
                if (index != -1)
                {
                    VariantPrefab variant = _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                    IBuilding staticBuilding = variant.GetBuilding(_prefabFactory);
                    _buildingDetails.ShowItemDetails(staticBuilding, variant);
                    _buildingDetails.GetBuildingVariantDetailController().variantName.text
                     = LocTableCache.CommonTable.GetString(StaticData.Variants[index].VariantNameStringKeyBase)
                      + " " + LocTableCache.CommonTable.GetString("Buildables/Buildings/" + building.keyName + "Name");
                    //_buildingDetails.GetBuildingVariantDetailController().variantDescription.text = _commonString.GetString(DataProvider.GameModel.Variants[index].variantDescriptionStringKeyBase);
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

        private void ShowItemDetailsV2(IUnit unit)
        {
            if (unit.Faction == Faction.Blues)
            {
                int index = DataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(_prefabFactory, unit);
                if (index != -1)
                {
                    VariantPrefab variant = _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                    IUnit staticUnit = variant.GetUnit(_prefabFactory);
                    _unitDetails.ShowItemDetails(staticUnit, variant);
                    _unitDetails.GetUnitVariantDetailController().variantName.text
                     = LocTableCache.CommonTable.GetString(StaticData.Variants[index].VariantNameStringKeyBase)
                      + " " + LocTableCache.CommonTable.GetString("Buildables/Units/" + unit.keyName + "Name");
                    //_unitDetails.GetUnitVariantDetailController().variantDescription.text = _commonString.GetString(DataProvider.GameModel.Variants[index].variantDescriptionStringKeyBase);
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
            else
            {
                int index = unit.variantIndex;
                if (index != -1)
                {
                    VariantPrefab variant = _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                    IUnit staticUnit = variant.GetUnit(_prefabFactory);
                    _unitDetails.ShowItemDetails(staticUnit, variant);
                    _unitDetails.GetUnitVariantDetailController().variantName.text
                     = LocTableCache.CommonTable.GetString(StaticData.Variants[index].VariantNameStringKeyBase)
                      + " " + LocTableCache.CommonTable.GetString("Buildables/Units/" + unit.keyName + "Name");
                    //_unitDetails.GetUnitVariantDetailController().variantDescription.text = _commonString.GetString(DataProvider.GameModel.Variants[index].variantDescriptionStringKeyBase);
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
