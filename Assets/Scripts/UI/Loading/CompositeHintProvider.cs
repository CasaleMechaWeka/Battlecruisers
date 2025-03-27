using BattleCruisers.Data.Models;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Loading
{
    public class CompositeHintProvider : IHintProvider
    {
        private readonly IHintProvider _basicHints, _advancedHints;
        private readonly GameModel _gameModel;

        // The level local boosters are unlocked (one of the advanced hints refers
        // to local boosters)
        public const int ADVANCED_HINT_LEVEL_REQUIREMENT = 9;

        public CompositeHintProvider(IHintProvider basicHints, IHintProvider advancedHints, GameModel gameModel)
        {
            Helper.AssertIsNotNull(basicHints, advancedHints, gameModel);

            _basicHints = basicHints;
            _advancedHints = advancedHints;
            _gameModel = gameModel;
        }

        public string GetHint()
        {
            if (_gameModel.NumOfLevelsCompleted > ADVANCED_HINT_LEVEL_REQUIREMENT
                && RandomGenerator.NextBool())
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