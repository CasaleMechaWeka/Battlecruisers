using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class ShieldStats : MonoBehaviour, IShieldStats
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
