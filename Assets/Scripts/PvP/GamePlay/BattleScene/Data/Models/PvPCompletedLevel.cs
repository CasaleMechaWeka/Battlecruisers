using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{
    [Serializable]
    public class PvPCompletedLevel : IPvPCompletedLevel
    {
        [SerializeField]
        private int _levelNum;

        [SerializeField]
        private Difficulty _hardestDifficulty;

        public int LevelNum
        {
            get { return _levelNum; }
            set { _levelNum = value; }
        }

        public Difficulty HardestDifficulty
        {
            get { return _hardestDifficulty; }
            set { _hardestDifficulty = value; }
        }

        public PvPCompletedLevel(int levelNum, Difficulty hardestDifficulty)
        {
            LevelNum = levelNum;
            HardestDifficulty = hardestDifficulty;
        }

        public override bool Equals(object obj)
        {
            PvPCompletedLevel other = obj as PvPCompletedLevel;
            return
                other != null
                && LevelNum == other.LevelNum
                && HardestDifficulty == other.HardestDifficulty;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(LevelNum, HardestDifficulty);
        }
    }
}