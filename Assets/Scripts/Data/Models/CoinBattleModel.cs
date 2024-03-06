using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
    [Serializable]
    public class CoinBattleModel : ICoinBattleModel
    {
        [SerializeField]
        private Difficulty _difficulty;
        public Difficulty Difficulty => _difficulty;

        [SerializeField]
        private HullKey _playerCruiser;
        public HullKey PlayerCruiser => _playerCruiser;

        public CoinBattleModel(
            Difficulty difficulty,
            HullKey playerCruiser)
        {
            Helper.AssertIsNotNull(playerCruiser);

            _difficulty = difficulty;
            _playerCruiser = playerCruiser;
        }
    }
}
