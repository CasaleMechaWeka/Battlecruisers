using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkProvider : ITrashTalkProvider
    {
        public async Task<TrashTalkData> GetTrashTalkAsync(int levelNum, bool isSideQuest = false)
        {
            if (isSideQuest)
            {
                IPrefabKey key = new LevelTrashTalkKey(levelNum, true);
                PrefabContainer<TrashTalkData> prefabContainer = await PrefabFetcher.GetPrefabAsync<TrashTalkData>(key);
                prefabContainer.Prefab.Initialise(true);
                return prefabContainer.Prefab;
            }
            else
            {
                IPrefabKey key = new LevelTrashTalkKey(levelNum);
                PrefabContainer<TrashTalkData> prefabContainer = await PrefabFetcher.GetPrefabAsync<TrashTalkData>(key);
                prefabContainer.Prefab.Initialise();
                return prefabContainer.Prefab;
            }
        }
    }
}