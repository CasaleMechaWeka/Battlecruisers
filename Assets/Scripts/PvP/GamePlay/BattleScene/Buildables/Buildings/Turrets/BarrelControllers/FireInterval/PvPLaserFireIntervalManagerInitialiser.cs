using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class PvPLaserFireIntervalManagerInitialiser : MonoBehaviour
    {
        public IPvPFireIntervalManager Initialise(IPvPDurationProvider waitingDurationProvider, IPvPDurationProvider firingDurationProvider)
        {
            PvPHelper.AssertIsNotNull(waitingDurationProvider, firingDurationProvider);

            PvPWaitingState waitingState = new PvPWaitingState();
            PvPFiringDurationState firingState = new PvPFiringDurationState();

            waitingState.Initialise(firingState, waitingDurationProvider);
            firingState.Initialise(waitingState, firingDurationProvider);

            return new PvPFireIntervalManager(firingState);
        }
    }
}
