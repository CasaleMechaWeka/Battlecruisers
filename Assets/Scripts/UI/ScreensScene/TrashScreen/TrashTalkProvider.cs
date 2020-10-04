using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkProvider : ITrashTalkProvider
    {
        private readonly IPrefabFetcher _prefabFetcher;

        public async Task<ITrashTalkData> GetTrashTalkAsync(int levelNum)
        {
            IPrefabKey key = new TrashTalkKey(levelNum);
            return await _prefabFetcher.GetPrefabAsync<TrashTalkData>(key);
        }
    }
}