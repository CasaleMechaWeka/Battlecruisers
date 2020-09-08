using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene
{
    /// <summary>
    /// Contains:
    /// + Drone number display
    /// + Cruiser health dial
    /// + Build menu
    /// </summary>
    public class LeftPanelInitialiser : MonoBehaviour
    {
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private FilterToggler _helpLabelsVisibilityToggler;
#pragma warning restore CS0414  // Variable is assigned but never used

        public DronesPanelInitialiser dronesPanelInitialiser;
        public BuildMenuInitialiser buildMenuInitialiser;
        public HelpLabel helpLabels;
        public GameObject popLimitReachedFeedback;

        public LeftPanelComponents Initialise(
            IDroneManager droneManager, 
            IDroneManagerMonitor droneManagerMonitor,
            IUIManager uiManager,
            ILoadout playerLoadout,
            IPrefabFactory prefabFactory,
            ISpriteProvider spriteProvider,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPrioritisedSoundPlayer eventSoundPlayer,
            ISingleSoundPlayer uiSoundPlayer,
            IPopulationLimitMonitor populationLimitMonitor,
            IStaticData staticData)
        {
            Helper.AssertIsNotNull(
                droneManager, 
                droneManagerMonitor, 
                uiManager,
                playerLoadout,
                prefabFactory,
                spriteProvider,
                buttonVisibilityFilters,
                playerCruiserFocusHelper,
                eventSoundPlayer,
                uiSoundPlayer,
                populationLimitMonitor,
                staticData);
            Helper.AssertIsNotNull(dronesPanelInitialiser, buildMenuInitialiser, helpLabels, popLimitReachedFeedback);

            IHighlightable numberOfDronesHighlightable = SetupDronesPanel(droneManager, droneManagerMonitor);
            IBuildMenu buildMenu 
                = SetupBuildMenuController(
                    uiManager, 
                    playerLoadout, 
                    prefabFactory, 
                    spriteProvider, 
                    buttonVisibilityFilters, 
                    playerCruiserFocusHelper, 
                    eventSoundPlayer, 
                    uiSoundPlayer, 
                    populationLimitMonitor,
                    staticData);
            SetupHelpLabels(buttonVisibilityFilters.HelpLabelsVisibilityFilter);

            return new LeftPanelComponents(numberOfDronesHighlightable, buildMenu, new GameObjectBC(popLimitReachedFeedback));
        }

        private IHighlightable SetupDronesPanel(IDroneManager droneManager, IDroneManagerMonitor droneManagerMonitor)
        {
            return dronesPanelInitialiser.Initialise(droneManager, droneManagerMonitor);
        }

        private IBuildMenu SetupBuildMenuController(
            IUIManager uiManager,
            ILoadout playerLoadout,
            IPrefabFactory prefabFactory,
            ISpriteProvider spriteProvider,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPrioritisedSoundPlayer eventSoundPlayer,
            ISingleSoundPlayer uiSoundPlayer,
            IPopulationLimitMonitor populationLimitMonitor,
            IStaticData staticData)
        {
            IBuildingGroupFactory buildingGroupFactory = new BuildingGroupFactory();
            IPrefabOrganiser prefabOrganiser = new PrefabOrganiser(playerLoadout, prefabFactory, buildingGroupFactory);
            IList<IBuildingGroup> buildingGroups = prefabOrganiser.GetBuildingGroups();
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units = prefabOrganiser.GetUnits();
            IBuildableSorterFactory sorterFactory 
                = new BuildableSorterFactory(
                    staticData,
                    new BuildableKeyFactory());

            return
                buildMenuInitialiser.Initialise(
                    uiManager,
                    buildingGroups,
                    units,
                    sorterFactory,
                    buttonVisibilityFilters,
                    spriteProvider,
                    playerCruiserFocusHelper,
                    eventSoundPlayer,
                    uiSoundPlayer,
                    populationLimitMonitor);
        }

        private void SetupHelpLabels(IBroadcastingFilter helpLabelsVisibilityFilter)
        {
            helpLabels.Initialise();
            _helpLabelsVisibilityToggler = new FilterToggler(helpLabelsVisibilityFilter, helpLabels);
        }
    }
}