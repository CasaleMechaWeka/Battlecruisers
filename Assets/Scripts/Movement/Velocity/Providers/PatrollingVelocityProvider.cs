using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity.Providers
{
    public class PatrollingVelocityProvider : IVelocityProvider
    {
        private readonly IPatrollingVelocityProvider _patrollingAircraft;

        public float VelocityInMPerS => _patrollingAircraft.PatrollingVelocityInMPerS;

        public PatrollingVelocityProvider(IPatrollingVelocityProvider patrollingAircraft)
        {
            Assert.IsNotNull(patrollingAircraft);
            _patrollingAircraft = patrollingAircraft;
        }
    }
}
