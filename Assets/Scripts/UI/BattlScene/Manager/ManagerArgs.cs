using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Manager
{
    public class ManagerArgs : IManagerArgs
    {
        public ICruiser PlayerCruiser { get; private set; }
        public ICruiser AICruiser { get; private set; }
        public IBuildMenu BuildMenu { get; private set; }
        public IBuildableDetailsManager DetailsManager { get; private set; }
        public IBroadcastingFilter<IBuilding> ShouldBuildingDeleteButtonBeEnabledFilter { get; private set; }

        public ManagerArgs(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IBuildMenu buildMenu,
            IBuildableDetailsManager detailsManager,
            IBroadcastingFilter<IBuilding> shouldBuildingDeleteButtonBeEnabledFilter)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, buildMenu, detailsManager, shouldBuildingDeleteButtonBeEnabledFilter);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;
            BuildMenu = buildMenu;
            DetailsManager = detailsManager;
            ShouldBuildingDeleteButtonBeEnabledFilter = shouldBuildingDeleteButtonBeEnabledFilter;
        }    
    }
}
