using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Data.Models;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Cruisers.Drones;

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

        /*        public async Task<PvPLeftPanelComponents> Initialise(
                    IDroneManager droneManager,
                    IPvPDroneManagerMonitor droneManagerMonitor,
                    IPvPUIManager uiManager,
                    ILoadout playerLoadout,
                    PvPPrefabFactory prefabFactory,
                    IPvPSpriteProvider spriteProvider,
                    IPvPButtonVisibilityFilters buttonVisibilityFilters,
                    IPlayerCruiserFocusHelper playerCruiserFocusHelper,
                    IPrioritisedSoundPlayer eventSoundPlayer,
                    IPvPSingleSoundPlayer uiSoundPlayer,
                    IPopulationLimitMonitor populationLimitMonitor,
                    StaticData staticData)
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

                    return new PvPLeftPanelComponents(numberOfDronesHighlightable, buildMenu, new GameObjectBC(popLimitReachedFeedback));
                }*/

        public PvPLeftPanelComponents Initialise(
            PvPCruiser playerCruiser,
            IPvPUIManager uiManager,
            ILoadout playerLoadout,
            PvPPrefabFactory prefabFactory,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPrioritisedSoundPlayer eventSoundPlayer,
            ISingleSoundPlayer uiSoundPlayer,
            IPopulationLimitMonitor populationLimitMonitor,
            bool flipClickAndDragIcon)
        {
            PvPHelper.AssertIsNotNull(
                playerCruiser,
                uiManager,
                playerLoadout,
                prefabFactory,
                buttonVisibilityFilters,
                playerCruiserFocusHelper,
                eventSoundPlayer,
                uiSoundPlayer,
                populationLimitMonitor);
            PvPHelper.AssertIsNotNull(dronesPanelInitialiser, buildMenuInitialiser, popLimitReachedFeedback);

            IHighlightable numberOfDronesHighlightable = SetupDronesPanel(playerCruiser);
            IPvPBuildMenu buildMenu
                = SetupBuildMenuController(
                    playerCruiser,
                    uiManager,
                    playerLoadout,
                    prefabFactory,
                    buttonVisibilityFilters,
                    playerCruiserFocusHelper,
                    eventSoundPlayer,
                    uiSoundPlayer,
                    populationLimitMonitor,
                    flipClickAndDragIcon);

            MakeLeftBackgroundPanelFit();

            return new PvPLeftPanelComponents(numberOfDronesHighlightable, buildMenu, new GameObjectBC(popLimitReachedFeedback));
        }

        private IHighlightable SetupDronesPanel(IDroneManager droneManager, DroneManagerMonitor droneManagerMonitor)
        {
            return dronesPanelInitialiser.Initialise(droneManager, droneManagerMonitor);
        }

        private IHighlightable SetupDronesPanel(PvPCruiser playerCruiser)
        {
            return dronesPanelInitialiser.Initialise(playerCruiser);
        }

        private IPvPBuildMenu SetupBuildMenuController(
            PvPCruiser playerCruiser,
            IPvPUIManager uiManager,
            ILoadout playerLoadout,
            PvPPrefabFactory prefabFactory,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPrioritisedSoundPlayer eventSoundPlayer,
            ISingleSoundPlayer uiSoundPlayer,
            IPopulationLimitMonitor populationLimitMonitor,
            bool flipClickAndDragIcon)
        {
            IPvPPrefabOrganiser prefabOrganiser = new PvPPrefabOrganiser(playerLoadout, prefabFactory);
            IList<IPvPBuildingGroup> buildingGroups = prefabOrganiser.GetBuildingGroups();
            IDictionary<UnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> units = prefabOrganiser.GetUnits();

            return
                buildMenuInitialiser.Initialise(
                    playerCruiser,
                    uiManager,
                    buildingGroups,
                    units,
                    buttonVisibilityFilters,
                    playerCruiserFocusHelper,
                    eventSoundPlayer,
                    uiSoundPlayer,
                    populationLimitMonitor,
                    flipClickAndDragIcon);
        }

        //Scales the LeftBackgroundPanel to a smaller size if the aspect ratio is 13.0/8 or lower
        //Warning: This is only called on initialisation, not on screen resize. Currently not a problem, but if screen resizing becomes a thing during a match, then this will need to be called.
        public void MakeLeftBackgroundPanelFit()
        {
            if ((double)Screen.width / Screen.height <= 13.0 / 8)
            {
                float scaleAmount = (float)((double)Screen.width / Screen.height / 1.667);
                //Debug.Log(scaleAmount);
                rectTransformThis.localScale = new Vector2(scaleAmount, scaleAmount);
            }
        }
    }
}