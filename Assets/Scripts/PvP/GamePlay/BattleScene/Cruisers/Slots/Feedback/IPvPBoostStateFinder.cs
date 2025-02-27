using BattleCruisers.Cruisers.Slots.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.Feedback
{
    public interface IPvPBoostStateFinder
    {
        BoostState FindState(int numOfLocalBoosters, IPvPBuilding building);
    }
}