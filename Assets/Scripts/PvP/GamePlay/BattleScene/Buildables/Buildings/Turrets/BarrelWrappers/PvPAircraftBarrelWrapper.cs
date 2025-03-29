using BattleCruisers.Buildables.Buildings.Turrets;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPAircraftBarrelWrapper : PvPLeadingDirectFireBarrelWrapper
    {
        [SerializeField]
        public string firingSoundKey; // This field can be set through Unity inspector


        protected override AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(-180, 180);
        }
    }
}
