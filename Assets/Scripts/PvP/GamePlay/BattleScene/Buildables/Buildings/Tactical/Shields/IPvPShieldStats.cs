using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields
{
    public interface IPvPShieldStats : IPvPBoostable
    {
        float ShieldRadiusInM { get; }
        float ShieldRechargeDelayInS { get; }
        float ShieldRechargeRatePerS { get; }
    }
}
