using BattleCruisers.Data.Static;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkDataList : MonoBehaviour, ITrashTalkProvider
    {
        private ITrashTalkData[] _trashDataList;

        public void Initialise()
        {
            _trashDataList = GetComponentsInChildren<ITrashTalkData>();
            Assert.AreEqual(StaticData.NUM_OF_LEVELS, _trashDataList.Length);
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