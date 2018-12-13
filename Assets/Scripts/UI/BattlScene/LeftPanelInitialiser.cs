using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Models;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
        public LeftPanelComponents Initialise(
            IDroneManager droneManager, 
            IDroneManagerMonitor droneManagerMonitor,
            IUIManager uiManager,
            ILoadout playerLoadout,
            IPrefabFactory prefabFactory,
            ISpriteProvider spriteProvider,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPrioritisedSoundPlayer soundPlayer,
            ICruiser playerCruiser)
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
                soundPlayer,
                playerCruiser);

            IMaskHighlightable numberOfDronesHighlightable = SetupDronesPanel(droneManager, droneManagerMonitor);
            IMaskHighlightable healthDialHighlightable = SetupHealthDial(playerCruiser);
            IBuildMenu buildMenu = SetupBuildMenuController(uiManager, playerLoadout, prefabFactory, spriteProvider, buttonVisibilityFilters, playerCruiserFocusHelper, soundPlayer);

            return new LeftPanelComponents(healthDialHighlightable, numberOfDronesHighlightable, buildMenu);
        }

        private IMaskHighlightable SetupDronesPanel(IDroneManager droneManager, IDroneManagerMonitor droneManagerMonitor)
        {
            DronesPanelInitialiser dronesPanelInitialiser = FindObjectOfType<DronesPanelInitialiser>();
            Assert.IsNotNull(dronesPanelInitialiser);
            return dronesPanelInitialiser.Initialise(droneManager, droneManagerMonitor);
        }

        private IMaskHighlightable SetupHealthDial(ICruiser playerCruiser)
        {
            PlayerCruiserHealthDialInitialiser dialInitialiser = GetComponentInChildren<PlayerCruiserHealthDialInitialiser>();
            Assert.IsNotNull(dialInitialiser);
            return dialInitialiser.Initialise(playerCruiser);
        }

        private IBuildMenu SetupBuildMenuController(
            IUIManager uiManager,
            ILoadout playerLoadout,
            IPrefabFactory prefabFactory,
            ISpriteProvider spriteProvider,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPrioritisedSoundPlayer soundPlayer)
        {
            IBuildingGroupFactory buildingGroupFactory = new BuildingGroupFactory();
            IPrefabOrganiser prefabOrganiser = new PrefabOrganiser(playerLoadout, prefabFactory, buildingGroupFactory);
            IList<IBuildingGroup> buildingGroups = prefabOrganiser.GetBuildingGroups();
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units = prefabOrganiser.GetUnits();
            IBuildableSorterFactory sorterFactory = new BuildableSorterFactory();

            BuildMenuInitialiser buildMenuInitialiser = FindObjectOfType<BuildMenuInitialiser>();
            Assert.IsNotNull(buildMenuInitialiser);

            return
                buildMenuInitialiser.Initialise(
                    uiManager,
                    buildingGroups,
                    units,
                    sorterFactory,
                    buttonVisibilityFilters,
                    spriteProvider,
                    playerCruiserFocusHelper,
                    soundPlayer);
        }
    }
}