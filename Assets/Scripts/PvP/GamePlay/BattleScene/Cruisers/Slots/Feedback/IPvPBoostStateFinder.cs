using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.Feedback
{
    public interface IPvPBoostStateFinder
    {
        PvPBoostState FindState(int numOfLocalBoosters, IPvPBuilding building);
    }
}