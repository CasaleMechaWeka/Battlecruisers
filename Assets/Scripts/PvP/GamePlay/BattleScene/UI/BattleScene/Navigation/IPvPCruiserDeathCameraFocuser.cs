using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public interface IPvPCruiserDeathCameraFocuser
    {
        void FocusOnLosingCruiser(IPvPCruiser losingCruiser);
    }
}