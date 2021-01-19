using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkProvider : ITrashTalkProvider
    {
        private readonly IPrefabFetcher _prefabFetcher;

        public TrashTalkProvider(IPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(prefabFetcher);
            _prefabFetcher = prefabFetcher;
        }

        public async Task<ITrashTalkData> GetTrashTalkAsync(int levelNum)
        {
            IPrefabKey key = new TrashTalkKey(levelNum);
            IPrefabContainer<TrashTalkData> prefabContainer = await _prefabFetcher.GetPrefabAsync<TrashTalkData>(key);
            return prefabContainer.Prefab;
        }
    }
}