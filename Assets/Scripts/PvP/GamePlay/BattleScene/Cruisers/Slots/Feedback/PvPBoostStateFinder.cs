using BattleCruisers.Cruisers.Slots.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.Feedback
{
    public class PvPBoostStateFinder : IPvPBoostStateFinder
    {
        public BoostState FindState(int numOfLocalBoosters, IPvPBuilding building)
        {
            Assert.IsTrue(numOfLocalBoosters >= 0);

            if (building == null
                || !building.IsBoostable
                || numOfLocalBoosters == 0)
            {
                return BoostState.Off;
            }
            else if (numOfLocalBoosters == 1)
            {
                return BoostState.Single;
            }
            else
            {
                return BoostState.Double;
            }
        }
    }
}