using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields
{
    public interface IPvPShieldStats : IBoostable
    {
        float ShieldRadiusInM { get; }
        float shieldRechargeDelayModifier { get; set; }
        float shieldRechargeRateModifier { get; set; }
        float ShieldRechargeDelayInS { get; }
        float ShieldRechargeRatePerS { get; }
    }
}
