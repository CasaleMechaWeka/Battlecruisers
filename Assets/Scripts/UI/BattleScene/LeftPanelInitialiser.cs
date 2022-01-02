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
using BattleCruisers.UI.Sound.Players;
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
        public DronesPanelInitialiser dronesPanelInitialiser;
        public BuildMenuInitialiser buildMenuInitialiser;
        public GameObject popLimitReachedFeedback;

        public RectTransform rectTransformThis;

        

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
            Helper.AssertIsNotNull(dronesPanelInitialiser, buildMenuInitialiser, popLimitReachedFeedback);

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

            makeLeftBackgroundPanelFit();

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

        //Scales the LeftBackgroundPanel to a smaller size if the aspect ratio is 4/3 or lower
        //Warning: This is only called on initialisation, not on screen resize. Currently not a problem, but if screen resizing becomes a thing during a match, then this will need to be called.
        public void makeLeftBackgroundPanelFit()
        {
            if ((double)Screen.width/Screen.height <= 4.0/3)
            {
                rectTransformThis.localScale = new Vector2(0.8f, 0.8f);
            }
        }
    }
}