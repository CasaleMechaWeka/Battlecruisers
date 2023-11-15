using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPAircraftBarrelWrapper : PvPLeadingDirectFireBarrelWrapper
    {
        [SerializeField]
        public string firingSoundKey; // This field can be set through Unity inspector


        protected override IPvPAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateDummyLimiter();
        }
    }
}
