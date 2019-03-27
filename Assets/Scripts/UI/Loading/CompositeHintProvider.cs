using BattleCruisers.Data.Models;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Loading
{
    public class CompositeHintProvider : IHintProvider
    {
        private readonly IHintProvider _basicHints, _advancedHints;
        private readonly IGameModel _gameModel;
        private readonly IRandomGenerator _random;

        // The level local boosters are unlocked (one of the advanced hints refers
        // to local boosters)
        public const int ADVANCED_HINT_LEVEL_REQUIREMENT = 9;

        public CompositeHintProvider(IHintProvider basicHints, IHintProvider advancedHints, IGameModel gameModel, IRandomGenerator random)
        {
            Helper.AssertIsNotNull(basicHints, advancedHints, gameModel, random);

            _basicHints = basicHints;
            _advancedHints = advancedHints;
            _gameModel = gameModel;
            _random = random;
        }

        public string GetHint()
        {
            if (_gameModel.NumOfLevelsCompleted > ADVANCED_HINT_LEVEL_REQUIREMENT
                && _random.NextBool())
            {
                return _advancedHints.GetHint();
            }
            else
            {
                return _basicHints.GetHint();
            }
        }
    }
}