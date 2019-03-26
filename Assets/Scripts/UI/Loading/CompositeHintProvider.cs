using BattleCruisers.Data.Models;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Loading
{
    public class CompositeHintProvider : IHintProvider
    {
        private readonly IHintProvider _basicHints, _advancedHints;
        private readonly IGameModel _gameModel;

        public const int ADVANCED_HINT_LEVEL_REQUIREMENT = 7;

        public CompositeHintProvider(IHintProvider basicHints, IHintProvider advancedHints, IGameModel gameModel)
        {
            Helper.AssertIsNotNull(basicHints, advancedHints, gameModel);

            _basicHints = basicHints;
            _advancedHints = advancedHints;
            _gameModel = gameModel;
        }

        public string GetHint()
        {
            if (_gameModel.NumOfLevelsCompleted > ADVANCED_HINT_LEVEL_REQUIREMENT)
            {
                // FELIX :D  Randomly choose between basic and advanced hints
                return "";
            }
            else
            {
                return _basicHints.GetHint();
            }
        }
    }
}