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
        private bool _wasRandomPlayerCruiser;
        public bool WasRandomPlayerCruiser => _wasRandomPlayerCruiser;

        [SerializeField]
        private HullKey _playerCruiser;
        public HullKey PlayerCruiser => _playerCruiser;

        [SerializeField]
        private bool _wasRandomAICruiser;
        public bool WasRandomAICruiser => _wasRandomAICruiser;

        [SerializeField]
        private HullKey _aiCruiser;
        public HullKey AICruiser => _aiCruiser;

        [SerializeField]
        private bool _wasRandomStrategy;
        public bool WasRandomStrategy => _wasRandomStrategy;

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
            bool wasRandomPlayerCruiser,
            HullKey playerCruiser,
            bool wasRandomCruiser,
            HullKey aiCruiser, 
            bool wasRandomStrategy,
            StrategyType aIStrategy,
            int backgroundLevelNum,
            string skyMaterialName)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser);
            Assert.IsTrue(backgroundLevelNum > 0);
            Assert.IsTrue(backgroundLevelNum <= StaticData.NUM_OF_LEVELS);
            Assert.IsFalse(string.IsNullOrEmpty(skyMaterialName));

            _difficulty = difficulty;
            _wasRandomPlayerCruiser = wasRandomPlayerCruiser;
            _playerCruiser = playerCruiser;
            _wasRandomAICruiser = wasRandomCruiser;
            _aiCruiser = aiCruiser;
            _wasRandomStrategy = wasRandomStrategy;
            _aiStrategy = aIStrategy;
            _backgroundLevelNum = backgroundLevelNum;
            _skyMaterialName = skyMaterialName;
        }

        public override bool Equals(object obj)
        {
            return
                obj is SkirmishModel other
                && Difficulty == other.Difficulty
                && WasRandomPlayerCruiser == other.WasRandomPlayerCruiser
                && PlayerCruiser == other.PlayerCruiser
                && WasRandomAICruiser == other.WasRandomAICruiser
                && AICruiser.SmartEquals(other.AICruiser)
                && WasRandomStrategy == other.WasRandomStrategy
                && AIStrategy == other.AIStrategy
                && BackgroundLevelNum == other.BackgroundLevelNum
                && SkyMaterialName == other.SkyMaterialName;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Difficulty, WasRandomPlayerCruiser, PlayerCruiser, WasRandomAICruiser, AICruiser, WasRandomStrategy, AIStrategy, BackgroundLevelNum, SkyMaterialName);
        }

        public override string ToString()
        {
            return base.ToString() + $"Difficulty: {Difficulty}  PlayerCruiser: {PlayerCruiser}  AICruiser: {AICruiser}  AIStrategy: {AIStrategy}  BackrgoundLevelNum: {BackgroundLevelNum}  SkyMaterialName: {SkyMaterialName}";
        }
    }
}