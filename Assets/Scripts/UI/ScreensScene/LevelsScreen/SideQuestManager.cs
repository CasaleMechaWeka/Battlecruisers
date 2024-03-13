using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.UI;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.AI;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Threading;


public class SideQuestManager : ElementWithClickSound
{
    private IDeferrer _deferrer;
    private IPrefabFactory _prefabFactory;
    private IBackgroundStatsProvider backgroundStatsProvider;
    private IPrefabFetcher prefabFetcher;
    private IDataProvider _dataProvider;
    public List<SideQuestData> SideQuests { get; private set; } = new List<SideQuestData>();

    public async void Initialize()
    {
        backgroundStatsProvider = new BackgroundStatsProvider(prefabFetcher);
        SideQuests.Add(new SideQuestData(false, StaticPrefabKeys.CaptainExos.GetCaptainExoKey(31), 3, null, StaticPrefabKeys.Hulls.Yeti, SoundKeys.Music.Background.Confusion, false, 0, SkyMaterials.Purple));
        // Add more quests as needed
    }

    /*public void CheckAndUnlockSideQuests(int completedLevel)
    {
        foreach (var quest in SideQuests.Where(q => q.UnlockAfterLevel == completedLevel))
        {
            UnlockQuest(quest);
        }
    }*/

    private void UnlockQuest(SideQuestData quest)
    {
        // Implementation for unlocking quest
    }

    /*public void CompleteSideQuest(string questName)
    {
        var quest = SideQuests.FirstOrDefault(q => q.QuestName == questName);
        if (quest != null && !quest.IsCompleted)
        {
            //quest.IsCompleted = true;
            //UnlockItem(quest.SideQuestReward);
        }
    }*/

    private void UnlockItem(BuildingKey reward)
    {
        // Implementation to unlock item
    }
    private IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum)
    {
        ILevelInfo levelInfo = new LevelInfo(aiCruiser, playerCruiser, _dataProvider.GameModel, _prefabFactory);
        IStrategyFactory strategyFactory = CreateStrategyFactory(currentLevelNum);
        IAIManager aiManager = new AIManager(_prefabFactory, _dataProvider, _deferrer, playerCruiser, strategyFactory);
        return aiManager.CreateAI(levelInfo, _dataProvider.SettingsManager.AIDifficulty);
    }
    protected virtual IStrategyFactory CreateStrategyFactory(int currentLevelNum)
    {
        return new DefaultStrategyFactory(_dataProvider.StaticData.Strategies, currentLevelNum);
    }
}
