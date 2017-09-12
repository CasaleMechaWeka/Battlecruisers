using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    // FELIX  Also move shield health here, currently using Target.Health : /
    public class ShieldStats : MonoBehaviour, IShieldStats
	{
		public float shieldRadiusInM;
		public float shieldRechargeDelayInS;
		public float shieldRechargeRatePerS;

        public float ShieldRadiusInM { get { return shieldRadiusInM; } }
        public float ShieldRechargeDelayInS { get { return shieldRechargeDelayInS; } }
        public float ShieldRechargeRatePerS { get { return BoostMultiplier * shieldRechargeRatePerS; } }

        public float BoostMultiplier { private get; set; }
	}
}
