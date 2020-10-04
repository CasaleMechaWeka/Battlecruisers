using BattleCruisers.Data.Static;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class TrashTalkKey : PrefabKey
    {
        protected override string PrefabPathPrefix => "Prefabs/ScreensScene/TrashScreen/";

        public TrashTalkKey(int levelNum)
            : base(CreatePrefabName(levelNum)) { }

        private static string CreatePrefabName(int levelNum)
        {
            Assert.IsTrue(levelNum > 0);
            Assert.IsTrue(levelNum <= StaticData.NUM_OF_LEVELS);

            return $"TrashTalkData ({levelNum}) Variant";
        }
    }
}
