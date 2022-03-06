using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Localisation;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkDataList : MonoBehaviour, ITrashTalkProvider
    {
        private ITrashTalkData[] _trashDataList;

        public void Initialise(ILocTable storyStrings)
        {
            Assert.IsNotNull(storyStrings);

            _trashDataList = GetComponentsInChildren<TrashTalkData>();
            Assert.AreEqual(StaticData.NUM_OF_LEVELS, _trashDataList.Length);
            //int cnt = 0;
            foreach (TrashTalkData trashTalk in _trashDataList)
            {
                trashTalk.Initialise(storyStrings);
                //Debug.Log(cnt++);
            }
        }

        public Task<ITrashTalkData> GetTrashTalkAsync(int levelNum)
        {
            int index = levelNum - 1;
            Assert.IsTrue(index >= 0);
            Assert.IsTrue(index < _trashDataList.Length);
            return Task.FromResult(_trashDataList[index]);
        }
    }
}