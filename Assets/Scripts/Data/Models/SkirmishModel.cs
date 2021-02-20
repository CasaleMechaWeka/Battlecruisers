using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models
{
    // FELIX  Preserve sky & background for retry => Have here?
    public class SkirmishModel : ISkirmishModel
    {
        public Difficulty Difficulty { get; }
        public IPrefabKey AICruiser { get; }
        public StrategyType AIStrategy { get; }

        public SkirmishModel(Difficulty difficulty, IPrefabKey aiCruiser, StrategyType aIStrategy)
        {
            Assert.IsNotNull(aiCruiser);

            Difficulty = difficulty;
            AICruiser = aiCruiser;
            AIStrategy = aIStrategy;
        }

        public override string ToString()
        {
            return base.ToString() + $"Difficulty: {Difficulty}  AICruiser: {AICruiser}  AIStrategy: {AIStrategy}";
        }
    }
}