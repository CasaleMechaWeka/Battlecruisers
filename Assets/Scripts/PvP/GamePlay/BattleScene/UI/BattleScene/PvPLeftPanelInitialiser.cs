using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting;
using BattleCruisers.Data.Models;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    /// <summary>
    /// Contains:
    /// + Drone number display
    /// + Cruiser health dial
    /// + Build menu
    /// </summary>
    public class PvPLeftPanelInitialiser : MonoBehaviour
    {
        public PvPDronesPanelInitialiser dronesPanelInitialiser;
        public PvPBuildMenuInitialiser buildMenuInitialiser;
        public GameObject popLimitReachedFeedback;

        public RectTransform rectTransformThis;



        public async Task<PvPLeftPanelComponents> Initialise(
            IPvPDroneManager droneManager,
            IPvPDroneManagerMonitor droneManagerMonitor,
            IPvPUIManager uiManager,
            ILoadout playerLoadout,
            IPvPPrefabFactory prefabFactory,
            IPvPSpriteProvider spriteProvider,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPvPPrioritisedSoundPlayer eventSoundPlayer,
            IPvPSingleSoundPlayer uiSoundPlayer,
            IPvPPopulationLimitMonitor populationLimitMonitor,
            IStaticData staticData)
        {
            PvPHelper.AssertIsNotNull(
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
            PvPHelper.AssertIsNotNull(dronesPanelInitialiser, buildMenuInitialiser, popLimitReachedFeedback);

            IPvPHighlightable numberOfDronesHighlightable = SetupDronesPanel(droneManager, droneManagerMonitor);
            IPvPBuildMenu buildMenu
                = await SetupBuildMenuController(
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

            return new PvPLeftPanelComponents(numberOfDronesHighlightable, buildMenu, new PvPGameObjectBC(popLimitReachedFeedback));
        }


        public async Task<PvPLeftPanelComponents> Initialise(
            PvPCruiser playerCruiser,
            IPvPUIManager uiManager,
            ILoadout playerLoadout,
            IPvPPrefabFactory prefabFactory,
            IPvPSpriteProvider spriteProvider,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPvPPrioritisedSoundPlayer eventSoundPlayer,
            IPvPSingleSoundPlayer uiSoundPlayer,
            IPvPPopulationLimitMonitor populationLimitMonitor,
            IStaticData staticData)
        {
            PvPHelper.AssertIsNotNull(
                playerCruiser,
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
            PvPHelper.AssertIsNotNull(dronesPanelInitialiser, buildMenuInitialiser, popLimitReachedFeedback);

            IPvPHighlightable numberOfDronesHighlightable = SetupDronesPanel(playerCruiser);
            IPvPBuildMenu buildMenu
                = await SetupBuildMenuController(
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

            return new PvPLeftPanelComponents(numberOfDronesHighlightable, buildMenu, new PvPGameObjectBC(popLimitReachedFeedback));
        }

        private IPvPHighlightable SetupDronesPanel(IPvPDroneManager droneManager, IPvPDroneManagerMonitor droneManagerMonitor)
        {
            return dronesPanelInitialiser.Initialise(droneManager, droneManagerMonitor);
        }

        private IPvPHighlightable SetupDronesPanel(PvPCruiser playerCruiser)
        {
            return dronesPanelInitialiser.Initialise(playerCruiser);
        }

        private async Task<IPvPBuildMenu> SetupBuildMenuController(
            IPvPUIManager uiManager,
            ILoadout playerLoadout,
            IPvPPrefabFactory prefabFactory,
            IPvPSpriteProvider spriteProvider,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPvPPrioritisedSoundPlayer eventSoundPlayer,
            IPvPSingleSoundPlayer uiSoundPlayer,
            IPvPPopulationLimitMonitor populationLimitMonitor,
            IStaticData staticData)
        {
            IPvPBuildingGroupFactory buildingGroupFactory = new PvPBuildingGroupFactory();
            IPvPPrefabOrganiser prefabOrganiser = new PvPPrefabOrganiser(playerLoadout, prefabFactory, buildingGroupFactory);
            IList<IPvPBuildingGroup> buildingGroups = await prefabOrganiser.GetBuildingGroups();
            IDictionary<PvPUnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> units = prefabOrganiser.GetUnits();
            IPvPBuildableSorterFactory sorterFactory
                = new PvPBuildableSorterFactory(
                    staticData,
                    new PvPBuildableKeyFactory());

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

        //Scales the LeftBackgroundPanel to a smaller size if the aspect ratio is 13.0/8 or lower
        //Warning: This is only called on initialisation, not on screen resize. Currently not a problem, but if screen resizing becomes a thing during a match, then this will need to be called.
        public void makeLeftBackgroundPanelFit()
        {
            if ((double)Screen.width / Screen.height <= 13.0 / 8)
            {
                float scaleAmount = (float)(((double)Screen.width / Screen.height) / 1.667);
                //Debug.Log(scaleAmount);
                rectTransformThis.localScale = new Vector2(scaleAmount, scaleAmount);
            }
        }
    }
}