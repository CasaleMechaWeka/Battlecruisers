using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields
{
    public class PvPShieldStats : MonoBehaviour, IPvPShieldStats
    {
        public float shieldRadiusInM;
        public float shieldRechargeDelayInS;
        public float shieldRechargeRatePerS;

        public float ShieldRadiusInM => shieldRadiusInM;
        public float ShieldRechargeDelayInS => shieldRechargeDelayInS;
        public float ShieldRechargeRatePerS => BoostMultiplier * shieldRechargeRatePerS;

        public float BoostMultiplier { get; set; }
    }
}
