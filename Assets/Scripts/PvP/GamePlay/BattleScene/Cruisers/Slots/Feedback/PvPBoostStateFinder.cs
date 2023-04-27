using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.Feedback
{
    public class PvPBoostStateFinder : IPvPBoostStateFinder
    {
        public PvPBoostState FindState(int numOfLocalBoosters, IPvPBuilding building)
        {
            Assert.IsTrue(numOfLocalBoosters >= 0);

            if (building == null
                || !building.IsBoostable
                || numOfLocalBoosters == 0)
            {
                return PvPBoostState.Off;
            }
            else if (numOfLocalBoosters == 1)
            {
                return PvPBoostState.Single;
            }
            else
            {
                return PvPBoostState.Double;
            }
        }
    }
}