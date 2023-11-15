using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class PvPFireIntervalManagerInitialiser : MonoBehaviour
    {
        public IPvPFireIntervalManager Initialise(IPvPDurationProvider sharedDurationProvider)
        {
            Assert.IsNotNull(sharedDurationProvider);

            PvPWaitingState waitingState = new PvPWaitingState();
            PvPFiringOnceState firingState = new PvPFiringOnceState();

            waitingState.Initialise(firingState, sharedDurationProvider);
            firingState.Initialise(waitingState, sharedDurationProvider);

            return new PvPFireIntervalManager(firingState);
        }
    }
}
