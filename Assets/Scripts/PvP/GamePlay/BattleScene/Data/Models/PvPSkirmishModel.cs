using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Utils;
using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{
    [Serializable]
    public class PvPSkirmishModel : IPvPSkirmishModel
    {
        [SerializeField]
        private Difficulty _difficulty;
        public Difficulty Difficulty => _difficulty;

        [SerializeField]
        private bool _wasRandomPlayerCruiser;
        public bool WasRandomPlayerCruiser => _wasRandomPlayerCruiser;

        [SerializeField]
        private PvPHullKey _playerCruiser;
        public PvPHullKey PlayerCruiser => _playerCruiser;

        [SerializeField]
        private bool _wasRandomAICruiser;
        public bool WasRandomAICruiser => _wasRandomAICruiser;

        [SerializeField]
        private PvPHullKey _aiCruiser;
        public PvPHullKey AICruiser => _aiCruiser;

        [SerializeField]
        private bool _wasRandomStrategy;
        public bool WasRandomStrategy => _wasRandomStrategy;

        [SerializeField]
        private StrategyType _aiStrategy;
        public StrategyType AIStrategy => _aiStrategy;

        [SerializeField]
        private int _backgroundLevelNum;
        public int BackgroundLevelNum => _backgroundLevelNum;

        public PvPSkirmishModel(
            Difficulty difficulty,
            bool wasRandomPlayerCruiser,
            PvPHullKey playerCruiser,
            bool wasRandomAICruiser,
            PvPHullKey aiCruiser,
            bool wasRandomStrategy,
            StrategyType aIStrategy,
            int backgroundLevelNum)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser);
            Assert.IsTrue(backgroundLevelNum > 0);
            Assert.IsTrue(backgroundLevelNum <= StaticData.NUM_OF_LEVELS);

            _difficulty = difficulty;
            _wasRandomPlayerCruiser = wasRandomPlayerCruiser;
            _playerCruiser = playerCruiser;
            _wasRandomAICruiser = wasRandomAICruiser;
            _aiCruiser = aiCruiser;
            _wasRandomStrategy = wasRandomStrategy;
            _aiStrategy = aIStrategy;
            _backgroundLevelNum = backgroundLevelNum;
        }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (_difficulty == Difficulty.Easy)
            {
                _difficulty = Difficulty.Normal;
            }
        }

        public override bool Equals(object obj)
        {
            return
                obj is PvPSkirmishModel other
                && Difficulty == other.Difficulty
                && WasRandomPlayerCruiser == other.WasRandomPlayerCruiser
                && PlayerCruiser.SmartEquals(other.PlayerCruiser)
                && WasRandomAICruiser == other.WasRandomAICruiser
                && AICruiser.SmartEquals(other.AICruiser)
                && WasRandomStrategy == other.WasRandomStrategy
                && AIStrategy == other.AIStrategy
                && BackgroundLevelNum == other.BackgroundLevelNum;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Difficulty, WasRandomPlayerCruiser, PlayerCruiser, WasRandomAICruiser, AICruiser, WasRandomStrategy, AIStrategy, BackgroundLevelNum);
        }

        public override string ToString()
        {
            return base.ToString() + $"Difficulty: {Difficulty}  PlayerCruiser: {PlayerCruiser}  AICruiser: {AICruiser}  AIStrategy: {AIStrategy}  BackrgoundLevelNum: {BackgroundLevelNum}";
        }
    }
}