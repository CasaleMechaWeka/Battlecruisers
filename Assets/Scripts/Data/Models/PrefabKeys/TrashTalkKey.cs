using UnityEngine;
using BattleCruisers.Data.Static;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class LevelTrashTalkKey : PrefabKey
    {
        protected override string PrefabPathPrefix => "Prefabs/ScreensScene/TrashScreen/";

        public LevelTrashTalkKey(int levelNum, bool isSideQuest = false)
            : base(CreatePrefabName(levelNum, isSideQuest)) { }

        private static string CreatePrefabName(int levelNum, bool isSideQuest)
        {
            if (isSideQuest)
            {
                Debug.Log(levelNum);
                Assert.IsTrue(levelNum >= 0);
                Assert.IsTrue(levelNum <= StaticData.NUM_OF_SIDEQUESTS);
                return $"SideQuestTrashTalkData ({levelNum}) Variant";
            }
            else
            {
                Assert.IsTrue(levelNum > 0);
                Assert.IsTrue(levelNum <= StaticData.NUM_OF_LEVELS);
                return $"TrashTalkData ({levelNum}) Variant";
            }
        }
    }
}
