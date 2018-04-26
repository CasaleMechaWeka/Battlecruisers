using System;
using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    /// <summary>
    /// Deems a buildable button as enabled if it's buildable can be afforded
    /// according to the drone manager's current number of drones.
    /// </summary>
    /// FELIX  Test :D
    public class AffordableDecider : IBuildableButtonActivenessDecider<IBuildable>
    {
        private readonly IDroneManager _droneManager;

		public event EventHandler PotentialActivenessChange;

        public AffordableDecider(IDroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);

            _droneManager = droneManager;
            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            if (PotentialActivenessChange != null)
            {
                PotentialActivenessChange.Invoke(this, EventArgs.Empty);
            }
        }

        public bool ShouldBeEnabled(IBuildable buildable)
        {
            return _droneManager.CanSupportDroneConsumer(buildable.NumOfDronesRequired);
        }
    }
}
