using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails.Buttons;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

// FELIX   Rename namespace to BuildableDetails :/
namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class BuildableBottomBarController : MonoBehaviour, IBuildableBottomBar
    {
        private RepairButtonController _repairButton;
        private ToggleDroneButtonController _toggleDronesButton;
        private BuildableProgressBarController _buildProgressController;

        public IBuildable Buildable
        {
            set 
            {
                IBuildable buildable = value;

                // If the buildable is not initialised, it does not exist in the
                // scene yet and this bottom bar does not need to be displayed.
                IsVisible = buildable != null && buildable.IsInitialised;

                _buildProgressController.Buildable = buildable;
                _toggleDronesButton.Buildable = buildable;
                _repairButton.Repairable = buildable;
            }
        }

        public bool IsVisible 
        { 
            get { return gameObject.activeSelf; } 
            private set { gameObject.SetActive(value); }
        }

        public float Height { get; private set; }

        public void Initialise(IDroneManager droneManager, IRepairManager repairManager)
        {
            Helper.AssertIsNotNull(droneManager, repairManager);

            RectTransform rectTransform = transform as RectTransform;
            Assert.IsNotNull(rectTransform);
            Height = rectTransform.sizeDelta.y;

            _repairButton = GetComponentInChildren<RepairButtonController>(includeInactive: true);
            Assert.IsNotNull(_repairButton);
            _repairButton.Initialise(droneManager, repairManager);

            _toggleDronesButton = GetComponentInChildren<ToggleDroneButtonController>(includeInactive: true);
            Assert.IsNotNull(_toggleDronesButton);
            _toggleDronesButton.Initialise();

            _buildProgressController = GetComponentInChildren<BuildableProgressBarController>(includeInactive: true);
            Assert.IsNotNull(_buildProgressController);
        }
    }
}
