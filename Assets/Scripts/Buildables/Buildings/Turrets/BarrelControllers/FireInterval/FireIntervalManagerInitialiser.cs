using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class FireIntervalManagerInitialiser : MonoBehaviour
	{
        public IFireIntervalManager Initialise(IDurationProvider waitingDurationProvider)
        {
            Assert.IsNotNull(waitingDurationProvider);

            WaitingState waitingState = new WaitingState();
            FiringOnceState firingState = new FiringOnceState();

            waitingState.Initialise(firingState, waitingDurationProvider);
            firingState.Initialise(waitingState);

            return new FireIntervalManager(firingState);
        }
   	}
}
