using System;
using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders
{
    /// <summary>
    /// Deems a buildable button as enabled if it's buildable can be afforded
    /// according to the drone manager's current number of drones.
    /// </summary>
    public class BuildableAffordableDecider : IFilter<IBuildable>
    {
        private readonly IDroneManager _droneManager;

		public event EventHandler PotentialMatchChange;

        public BuildableAffordableDecider(IDroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);

            _droneManager = droneManager;
            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            if (PotentialMatchChange != null)
            {
                PotentialMatchChange.Invoke(this, EventArgs.Empty);
            }
        }

        public bool IsMatch(IBuildable buildable)
        {
            return _droneManager.CanSupportDroneConsumer(buildable.NumOfDronesRequired);
        }
    }
}
