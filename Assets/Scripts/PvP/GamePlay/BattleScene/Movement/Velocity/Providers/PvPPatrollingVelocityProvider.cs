using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers
{
    public class PvPPatrollingVelocityProvider : IPvPVelocityProvider
    {
        private readonly IPvPPatrollingVelocityProvider _patrollingAircraft;

        public float VelocityInMPerS => _patrollingAircraft.PatrollingVelocityInMPerS;

        public PvPPatrollingVelocityProvider(IPvPPatrollingVelocityProvider patrollingAircraft)
        {
            Assert.IsNotNull(patrollingAircraft);
            _patrollingAircraft = patrollingAircraft;
        }
    }
}
