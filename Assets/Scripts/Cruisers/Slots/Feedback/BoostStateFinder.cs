using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots.Feedback
{
    // FELIX  Use, test
    public class BoostStateFinder : IBoostStateFinder
    {
        public BoostState FindState(int numOfLocalBoosters, bool isBuildingBoostable)
        {
            Assert.IsTrue(numOfLocalBoosters >= 0);

            if (!isBuildingBoostable
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