using BattleCruisers.Data.Static;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    public class PvPBackgroundImageStatsKey : PvPPrefabKey
    {
        private const string BACKGROUND_PATH = "Clouds/Background";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + BACKGROUND_PATH + PATH_SEPARATOR;
            }
        }

        public PvPBackgroundImageStatsKey(int levelNum)
            : base(CreatePrefabName(levelNum)) { }

        private static string CreatePrefabName(int levelNum)
        {
            Assert.IsTrue(levelNum > 0);
            Assert.IsTrue(levelNum <= StaticData.NUM_OF_LEVELS);

            return $"PvPBackgroundStatsLevel ({levelNum}) Variant";
        }
    }
}
