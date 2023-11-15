using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager
{
    public class PvPManagerArgs
    {
        public PvPCruiser PlayerCruiser { get; }
        public PvPCruiser EnemyCruiser { get; }
        public IPvPBuildMenu BuildMenu { get; }
        public IPvPItemDetailsManager DetailsManager { get; }
        public IPvPPrioritisedSoundPlayer SoundPlayer { get; }
        public IPvPSingleSoundPlayer UISoundPlayer { get; }

        public PvPManagerArgs(
            PvPCruiser playerCruiser,
            PvPCruiser enemyCruiser,
            IPvPBuildMenu buildMenu,
            IPvPItemDetailsManager detailsManager,
            IPvPPrioritisedSoundPlayer soundPlayer,
            IPvPSingleSoundPlayer uiSoundPlayer)
        {
            PvPHelper.AssertIsNotNull(playerCruiser, enemyCruiser, buildMenu, detailsManager, soundPlayer, uiSoundPlayer);

            PlayerCruiser = playerCruiser;
            EnemyCruiser = enemyCruiser;
            BuildMenu = buildMenu;
            DetailsManager = detailsManager;
            SoundPlayer = soundPlayer;
            UISoundPlayer = uiSoundPlayer;
        }
    }
}
