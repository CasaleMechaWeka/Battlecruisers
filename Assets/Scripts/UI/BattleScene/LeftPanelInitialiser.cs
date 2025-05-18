using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Models;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
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
        public DronesPanelInitialiser dronesPanelInitialiser;
        public BuildMenuInitialiser buildMenuInitialiser;
        public GameObject popLimitReachedFeedback;

        public LeftPanelComponents Initialise(
            DroneManager droneManager,
            DroneManagerMonitor droneManagerMonitor,
            UIManager uiManager,
            ILoadout playerLoadout,
            ButtonVisibilityFilters buttonVisibilityFilters,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPrioritisedSoundPlayer eventSoundPlayer,
            SingleSoundPlayer uiSoundPlayer,
            IPopulationLimitMonitor populationLimitMonitor)
        {
            Helper.AssertIsNotNull(
                droneManager,
                droneManagerMonitor,
                uiManager,
                playerLoadout,
                buttonVisibilityFilters,
                playerCruiserFocusHelper,
                eventSoundPlayer,
                uiSoundPlayer,
                populationLimitMonitor);
            Helper.AssertIsNotNull(dronesPanelInitialiser, buildMenuInitialiser, popLimitReachedFeedback);

            IHighlightable numberOfDronesHighlightable = SetupDronesPanel(droneManager, droneManagerMonitor);
            BuildMenu buildMenu
                = SetupBuildMenuController(
                    uiManager,
                    playerLoadout,
                    buttonVisibilityFilters,
                    playerCruiserFocusHelper,
                    eventSoundPlayer,
                    uiSoundPlayer,
                    populationLimitMonitor);


            return new LeftPanelComponents(numberOfDronesHighlightable, buildMenu, new GameObjectBC(popLimitReachedFeedback));
        }

        private IHighlightable SetupDronesPanel(DroneManager droneManager, DroneManagerMonitor droneManagerMonitor)
        {
            return dronesPanelInitialiser.Initialise(droneManager, droneManagerMonitor);
        }

        private BuildMenu SetupBuildMenuController(
            UIManager uiManager,
            ILoadout playerLoadout,
            ButtonVisibilityFilters buttonVisibilityFilters,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPrioritisedSoundPlayer eventSoundPlayer,
            SingleSoundPlayer uiSoundPlayer,
            IPopulationLimitMonitor populationLimitMonitor)
        {
            PrefabOrganiser prefabOrganiser = new PrefabOrganiser(playerLoadout);
            IList<IBuildingGroup> buildingGroups = prefabOrganiser.GetBuildingGroups();
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units = prefabOrganiser.GetUnits();

            return
                buildMenuInitialiser.Initialise(
                    uiManager,
                    buildingGroups,
                    units,
                    buttonVisibilityFilters,
                    playerCruiserFocusHelper,
                    eventSoundPlayer,
                    uiSoundPlayer,
                    populationLimitMonitor);
        }
    }
}