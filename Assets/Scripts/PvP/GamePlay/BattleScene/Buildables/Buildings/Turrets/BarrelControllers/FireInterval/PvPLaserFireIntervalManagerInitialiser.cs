using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class PvPLaserFireIntervalManagerInitialiser : MonoBehaviour
    {
        public FireIntervalManager Initialise(IDurationProvider waitingDurationProvider, IDurationProvider firingDurationProvider)
        {
            PvPHelper.AssertIsNotNull(waitingDurationProvider, firingDurationProvider);

            PvPWaitingState waitingState = new PvPWaitingState();
            PvPFiringDurationState firingState = new PvPFiringDurationState();

            waitingState.Initialise(firingState, waitingDurationProvider);
            firingState.Initialise(waitingState, firingDurationProvider);

            return new FireIntervalManager(firingState);
        }
    }
}
