using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public interface IShieldStats : IBoostable
    {
        float ShieldRadiusInM { get; }
        float shieldRechargeDelayModifier { get; set; }
        float shieldRechargeRateModifier { get; set; }

        float ShieldRechargeDelayInS { get; }
        float ShieldRechargeRatePerS { get; }
    }
}
