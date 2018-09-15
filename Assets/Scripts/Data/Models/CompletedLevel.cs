using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
    // FELIX  Create serialize tests :)
    [Serializable]
    public class CompletedLevel : ICompletedLevel
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

        public CompletedLevel(int levelNum, Difficulty hardestCompletedDifficulty)
        {
            LevelNum = levelNum;
            HardestDifficulty = hardestCompletedDifficulty;
        }

        public override bool Equals(object obj)
        {
            CompletedLevel other = obj as CompletedLevel;
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