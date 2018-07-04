using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class LaserFireIntervalManagerInitialiser : MonoBehaviour
	{
		public IFireIntervalManager Initialise(IDurationProvider waitingDurationProvider, IDurationProvider firingDurationProvider)
		{
            Helper.AssertIsNotNull(waitingDurationProvider, firingDurationProvider);

            WaitingState waitingState = new WaitingState();
            FiringDurationState firingState = new FiringDurationState();

            waitingState.Initialise(firingState, waitingDurationProvider);
            firingState.Initialise(waitingState, firingDurationProvider);

            return new FireIntervalManager(firingState);
		}
    }
}
