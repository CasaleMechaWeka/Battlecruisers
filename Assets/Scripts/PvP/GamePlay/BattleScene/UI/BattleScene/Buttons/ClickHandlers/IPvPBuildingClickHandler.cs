using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers
{
    public interface IPvPBuildingClickHandler
    {
        void HandleClick(bool canAffordBuildable, IPvPBuildableWrapper<IPvPBuilding> buildingClicked);
        void HandleHover(IPvPBuildableWrapper<IPvPBuilding> buildingClicked);
        void HandleHoverExit();
    }
}