using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public class PvPLinearProportionAssigner : IPvPAssigner
    {
        private readonly int _numOfOptions;

        private const int MIN_NUM_OF_OPTIONS = 1;
        private const int MIN_PROPORTION = 0;
        private const int MAX_PROPORTION = 1;

        public PvPLinearProportionAssigner(int numOfOptions)
        {
            Assert.IsTrue(numOfOptions >= MIN_NUM_OF_OPTIONS);
            _numOfOptions = numOfOptions;
        }

        public int Assign(float proportion)
        {
            Assert.IsTrue(proportion >= MIN_PROPORTION);
            Assert.IsTrue(proportion <= MAX_PROPORTION);

            // Invert, because of sprite order 
            float invertedProprtion = 1 - proportion;

            int index = (int)(invertedProprtion * _numOfOptions);

            if (index == _numOfOptions)
            {
                index -= 1;
            }

            return index;
        }
    }
}
