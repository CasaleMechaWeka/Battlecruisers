using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public interface IPvPItemDetailsManager
    {
        IPvPBroadcastingProperty<IPvPTarget> SelectedItem { get; }

        void ShowDetails(IPvPBuilding building);
        void ShowDetails(IPvPUnit unit);
        void ShowDetails(IPvPCruiser cruiser);
        void SelectUnit(IPvPUnit unit);
        void SelectBuilding(IPvPBuilding building);
        void HideDetails();
    }
}
