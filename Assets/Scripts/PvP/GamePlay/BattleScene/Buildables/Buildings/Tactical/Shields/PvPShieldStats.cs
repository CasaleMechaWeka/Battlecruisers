using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields
{
    public class PvPShieldStats : MonoBehaviour, IShieldStats
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
