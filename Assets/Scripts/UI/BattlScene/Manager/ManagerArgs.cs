using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Manager
{
    public class ManagerArgs
    {
        public ICruiser PlayerCruiser { get; }
        public ICruiser AICruiser { get; }
        public IBuildMenu BuildMenu { get; }
        public IItemDetailsManager DetailsManager { get; }
        public IPrioritisedSoundPlayer SoundPlayer { get; }
        public ISingleSoundPlayer UISoundPlayer { get; }

        public ManagerArgs(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IBuildMenu buildMenu,
            IItemDetailsManager detailsManager,
            IPrioritisedSoundPlayer soundPlayer,
            ISingleSoundPlayer uiSoundPlayer)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, buildMenu, detailsManager, soundPlayer, uiSoundPlayer);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;
            BuildMenu = buildMenu;
            DetailsManager = detailsManager;
            SoundPlayer = soundPlayer;
            UISoundPlayer = uiSoundPlayer;
        }    
    }
}
