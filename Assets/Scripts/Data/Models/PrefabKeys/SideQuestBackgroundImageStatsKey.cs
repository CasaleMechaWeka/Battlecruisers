using BattleCruisers.Data.Static;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class SideQuestBackgroundImageStatsKey : PrefabKey
    {
        private const string BACKGROUND_PATH = "Clouds/Background";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + BACKGROUND_PATH + PATH_SEPARATOR;
            }
        }

        public SideQuestBackgroundImageStatsKey(int levelNum)
            : base(CreatePrefabName(levelNum)) { }

        private static string CreatePrefabName(int levelNum)
        {
            Assert.IsTrue(levelNum <= StaticData.NUM_OF_SIDEQUESTS);

            return $"BackgroundStatsSideQuest ({levelNum}) Variant";
        }
    }
}
