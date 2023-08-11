using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class FireIntervalManagerInitialiser : MonoBehaviour
	{
        public IFireIntervalManager Initialise(IDurationProvider sharedDurationProvider)
        {
            Assert.IsNotNull(sharedDurationProvider);

            WaitingState waitingState = new WaitingState();
            FiringOnceState firingState = new FiringOnceState();
            
            waitingState.Initialise(firingState, sharedDurationProvider);
            firingState.Initialise(waitingState, sharedDurationProvider);

            return new FireIntervalManager(firingState);
        }
   	}
}
