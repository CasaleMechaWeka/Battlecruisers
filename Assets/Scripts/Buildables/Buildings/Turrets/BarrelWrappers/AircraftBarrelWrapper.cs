using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class AircraftBarrelWrapper : LeadingDirectFireBarrelWrapper
    {
        [SerializeField]
        public string firingSoundKey; // This field can be set through Unity inspector


        protected override IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateDummyLimiter();
        }
    }
}
