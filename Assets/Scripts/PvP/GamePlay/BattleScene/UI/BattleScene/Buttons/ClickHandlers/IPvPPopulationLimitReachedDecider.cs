using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers
{
    public interface IPvPPopulationLimitReachedDecider
    {
        bool ShouldPlayPopulationLimitReachedWarning(IPvPFactory factory);
    }
}