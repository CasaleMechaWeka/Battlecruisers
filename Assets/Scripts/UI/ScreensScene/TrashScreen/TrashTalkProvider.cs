using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System.Threading.Tasks;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkProvider : ITrashTalkProvider
    {
        private readonly ILocTable _storyStrings;

        public TrashTalkProvider(ILocTable storyStrings)
        {
            Helper.AssertIsNotNull(storyStrings);

            _storyStrings = storyStrings;
        }

        public async Task<ITrashTalkData> GetTrashTalkAsync(int levelNum, bool isSideQuest = false)
        {
            if (isSideQuest)
            {
                IPrefabKey key = new LevelTrashTalkKey(levelNum, true);
                PrefabContainer<TrashTalkData> prefabContainer = await PrefabFetcher.GetPrefabAsync<TrashTalkData>(key);
                prefabContainer.Prefab.Initialise(_storyStrings, true);
                return prefabContainer.Prefab;
            }
            else
            {
                IPrefabKey key = new LevelTrashTalkKey(levelNum);
                PrefabContainer<TrashTalkData> prefabContainer = await PrefabFetcher.GetPrefabAsync<TrashTalkData>(key);
                prefabContainer.Prefab.Initialise(_storyStrings);
                return prefabContainer.Prefab;
            }
        }
    }
}