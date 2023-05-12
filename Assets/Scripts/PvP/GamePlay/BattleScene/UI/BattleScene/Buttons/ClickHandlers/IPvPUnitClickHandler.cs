using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers
{
    public interface IPvPUnitClickHandler
    {
        void HandleClick(bool canAffordBuildable, IPvPBuildableWrapper<IPvPUnit> unitClicked, IPvPFactory unitFactory);
        void HandleHover(IPvPBuildableWrapper<IPvPUnit> unitClicked);

        void HandleHoverExit();
    }
}