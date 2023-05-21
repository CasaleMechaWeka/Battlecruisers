using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public interface IShieldStats : IBoostable
    {
        float ShieldRadiusInM { get; }
		float ShieldRechargeDelayInS { get; }
		float ShieldRechargeRatePerS { get; }
    }
}
