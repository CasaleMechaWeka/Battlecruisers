using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public class PvPItemDetailsManager : IPvPItemDetailsManager
    {
        private readonly IPvPInformatorPanel _informatorPanel;
        private readonly IPvPComparableItemDetails<IPvPBuilding> _buildingDetails;
        private readonly IPvPComparableItemDetails<IPvPUnit> _unitDetails;
        private readonly IPvPComparableItemDetails<IPvPCruiser> _cruiserDetails;

        private IPvPSettableBroadcastingProperty<IPvPTarget> _selectedItem;
        public IPvPBroadcastingProperty<IPvPTarget> SelectedItem { get; }

        public PvPItemDetailsManager(IPvPInformatorPanel informator)
        {
            PvPHelper.AssertIsNotNull(informator);

            _informatorPanel = informator;
            _buildingDetails = informator.BuildingDetails;
            _unitDetails = informator.UnitDetails;
            _cruiserDetails = informator.CruiserDetails;

            _selectedItem = new PvPSettableBroadcastingProperty<IPvPTarget>(initialValue: null);
            SelectedItem = new PvPBroadcastingProperty<IPvPTarget>(_selectedItem);
        }

        public void ShowDetails(IPvPBuilding building)
        {
            HideInformatorContent();

            _informatorPanel.Show(building);
            _buildingDetails.ShowItemDetails(building);
            _selectedItem.Value = building;
        }

        public void SelectBuilding(IPvPBuilding building)
        {
            _selectedItem.Value = building;
        }

        public void ShowDetails(IPvPUnit unit)
        {
            HideInformatorContent();

            _informatorPanel.Show(unit);
            _unitDetails.ShowItemDetails(unit);
            _selectedItem.Value = unit;
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
