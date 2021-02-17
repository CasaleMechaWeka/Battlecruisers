using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Skirmishes
{
    public class Skirmish : ISkirmish
    {
        public Difficulty Difficulty { get; }
        public IPrefabKey AICruiser { get; }
        public StrategyType AIStrategy { get; }

        public Skirmish(Difficulty difficulty, IPrefabKey aICruiser, StrategyType aIStrategy)
        {
            Assert.IsNotNull(AICruiser);

            Difficulty = difficulty;
            AICruiser = aICruiser;
            AIStrategy = aIStrategy;
        }
    }
}