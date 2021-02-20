using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models
{
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

        [SerializeField]
        private int _backgroundLevelNum;
        public int BackgroundLevelNum => _backgroundLevelNum;

        [SerializeField]
        private string _skyMaterialName;
        public string SkyMaterialName => _skyMaterialName;

        public SkirmishModel(
            Difficulty difficulty, 
            HullKey aiCruiser, 
            StrategyType aIStrategy,
            int backgroundLevelNum,
            string skyMaterialName)
        {
            Assert.IsNotNull(aiCruiser);
            Assert.IsTrue(backgroundLevelNum > 0);
            Assert.IsTrue(backgroundLevelNum <= StaticData.NUM_OF_LEVELS);
            Assert.IsFalse(string.IsNullOrEmpty(skyMaterialName));

            _difficulty = difficulty;
            _aiCruiser = aiCruiser;
            _aiStrategy = aIStrategy;
            _backgroundLevelNum = backgroundLevelNum;
            _skyMaterialName = skyMaterialName;
        }

        public override bool Equals(object obj)
        {
            return
                obj is SkirmishModel other
                && Difficulty == other.Difficulty
                && AICruiser.SmartEquals(other.AICruiser)
                && AIStrategy == other.AIStrategy
                && BackgroundLevelNum == other.BackgroundLevelNum
                && SkyMaterialName == other.SkyMaterialName;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Difficulty, AICruiser, AIStrategy, BackgroundLevelNum, SkyMaterialName);
        }

        public override string ToString()
        {
            return base.ToString() + $"Difficulty: {Difficulty}  AICruiser: {AICruiser}  AIStrategy: {AIStrategy}  BackrgoundLevelNum: {BackgroundLevelNum}  SkyMaterialName: {SkyMaterialName}";
        }
    }
}