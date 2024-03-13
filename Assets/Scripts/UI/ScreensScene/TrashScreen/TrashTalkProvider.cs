using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System.Threading.Tasks;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkProvider : ITrashTalkProvider
    {
        private readonly IPrefabFetcher _prefabFetcher;
        private readonly ILocTable _storyStrings;

        public TrashTalkProvider(IPrefabFetcher prefabFetcher, ILocTable storyStrings)
        {
            Helper.AssertIsNotNull(prefabFetcher, storyStrings);

            _prefabFetcher = prefabFetcher;
            _storyStrings = storyStrings;
        }

        public async Task<ITrashTalkData> GetTrashTalkAsync(int levelNum, bool isSideQuest = false)
        {
            if (isSideQuest)
            {
                IPrefabKey key = new LevelTrashTalkKey(levelNum, true);
                IPrefabContainer<TrashTalkData> prefabContainer = await _prefabFetcher.GetPrefabAsync<TrashTalkData>(key);
                prefabContainer.Prefab.Initialise(_storyStrings, true);
                return prefabContainer.Prefab;
            }
            else
            {
                IPrefabKey key = new LevelTrashTalkKey(levelNum);
                IPrefabContainer<TrashTalkData> prefabContainer = await _prefabFetcher.GetPrefabAsync<TrashTalkData>(key);
                prefabContainer.Prefab.Initialise(_storyStrings);
                return prefabContainer.Prefab;
            }
        }
    }
}