using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class ShieldStats : MonoBehaviour, IShieldStats
    {
        public float shieldRadiusInM;
        public float shieldRechargeDelayInS;
        public float shieldRechargeRatePerS;

        public float shieldRechargeDelayModifier { get; set; }
        public float shieldRechargeRateModifier { get; set; }

        public float ShieldRadiusInM => shieldRadiusInM;
        public float ShieldRechargeDelayInS => shieldRechargeDelayInS + shieldRechargeDelayModifier;
        public float ShieldRechargeRatePerS => BoostMultiplier * (shieldRechargeRatePerS + shieldRechargeRateModifier);

        public float BoostMultiplier { get; set; }
    }
}
