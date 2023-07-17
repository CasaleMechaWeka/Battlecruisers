using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers
{
    public interface IPvPPopulationLimitReachedDecider
    {
        bool ShouldPlayPopulationLimitReachedWarning(PvPCruiser playerCruiser, IPvPFactory factory);
    }
}