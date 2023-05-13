using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{
    [Serializable]
    public class PvPBattleResult
    {
        [SerializeField]
        private int _levelNum;

        [SerializeField]
        private bool _wasVictory;

        public int LevelNum
        {
            get { return _levelNum; }
            set { _levelNum = value; }
        }

        public bool WasVictory
        {
            get { return _wasVictory; }
            set { _wasVictory = value; }
        }

        public PvPBattleResult(int levelNum, bool wasVictory)
        {
            LevelNum = levelNum;
            WasVictory = wasVictory;
        }

        public override bool Equals(object obj)
        {
            PvPBattleResult other = obj as PvPBattleResult;
            return other != null
                && other.LevelNum == LevelNum
                && other.WasVictory == WasVictory;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(LevelNum, WasVictory);
        }
    }
}
