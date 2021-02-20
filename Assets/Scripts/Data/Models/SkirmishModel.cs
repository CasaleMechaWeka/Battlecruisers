using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models
{
    // FELIX  Add to GameModel :)
    // FELIX  Preserve sky & background for retry => Have here?
    [Serializable]
    public class SkirmishModel : ISkirmishModel
    {
        [SerializeField]
        private Difficulty _difficulty;
        public Difficulty Difficulty => _difficulty;

        [SerializeField]
        private HullKey _aiCruiser;
        public HullKey AICruiser => _aiCruiser;

        [SerializeField]
        private StrategyType _aiStrategy;
        public StrategyType AIStrategy => _aiStrategy;

        public SkirmishModel(Difficulty difficulty, HullKey aiCruiser, StrategyType aIStrategy)
        {
            Assert.IsNotNull(aiCruiser);

            _difficulty = difficulty;
            _aiCruiser = aiCruiser;
            _aiStrategy = aIStrategy;
        }

        public override bool Equals(object obj)
        {
            return
                obj is SkirmishModel other
                && Difficulty == other.Difficulty
                && AICruiser.SmartEquals(other.AICruiser)
                && AIStrategy == other.AIStrategy;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Difficulty, AICruiser, AIStrategy);
        }

        public override string ToString()
        {
            return base.ToString() + $"Difficulty: {Difficulty}  AICruiser: {AICruiser}  AIStrategy: {AIStrategy}";
        }
    }
}