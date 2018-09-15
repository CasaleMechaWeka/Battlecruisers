using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
    // FELIX  Create serialize tests :)
    [Serializable]
    public class LevelModel : ILevelModel
    {
        [SerializeField]
        private int _levelNum;

        [SerializeField]
        private Difficulty _hardestCompletedDifficulty;

        public int LevelNum
        {
            get { return _levelNum; }
            set { _levelNum = value; }
        }

        public Difficulty HardestCompletedDifficulty
        {
            get { return _hardestCompletedDifficulty; }
            set { _hardestCompletedDifficulty = value; }
        }

        public LevelModel(int levelNum, Difficulty hardestCompletedDifficulty)
        {
            LevelNum = levelNum;
            HardestCompletedDifficulty = hardestCompletedDifficulty;
        }

        public override bool Equals(object obj)
        {
            LevelModel other = obj as LevelModel;
            return
                other != null
                && LevelNum == other.LevelNum
                && HardestCompletedDifficulty == other.HardestCompletedDifficulty;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(LevelNum, HardestCompletedDifficulty);
        }
    }
}