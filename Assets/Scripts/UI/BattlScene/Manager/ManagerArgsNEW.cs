using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Manager
{
    // FELIX  Rename to UIManagerARgs :)
    public class ManagerArgsNEW
    {
        public ICruiser PlayerCruiser { get; private set; }
        public ICruiser AICruiser { get; private set; }
        public IBuildMenu BuildMenu { get; private set; }
        public IItemDetailsManager DetailsManager { get; private set; }

        public ManagerArgsNEW(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IBuildMenu buildMenu,
            IItemDetailsManager detailsManager)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, buildMenu, detailsManager);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;
            BuildMenu = buildMenu;
            DetailsManager = detailsManager;
        }    
    }
}
