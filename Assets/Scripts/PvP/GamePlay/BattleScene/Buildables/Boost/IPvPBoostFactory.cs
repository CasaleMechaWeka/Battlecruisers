using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    public interface IPvPBoostFactory
    {
        IBoostConsumer CreateBoostConsumer();
        IBoostProvider CreateBoostProvider(float boostMultiplier);
        IPvPBoostableGroup CreateBoostableGroup();
        IBoostable CreateBoostable();
    }
}