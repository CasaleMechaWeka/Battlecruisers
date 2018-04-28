using BattleCruisers.AI;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Scenes
{
    public class NormalHelper : IBattleSceneHelper
    {
        private readonly IDataProvider _dataProvider;
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDeferrer _deferrer;

        public NormalHelper(IDataProvider dataProvider, IPrefabFactory prefabFactory, IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory, deferrer);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
        }

        public IUIManager CreateUIManager(IManagerArgs args)
        {
            return new UIManager(args);
        }

        public ILoadout GetPlayerLoadout()
        {
            return _dataProvider.GameModel.PlayerLoadout;
        }
		
        public void CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum)
		{
            ILevelInfo levelInfo = new LevelInfo(aiCruiser, playerCruiser, _dataProvider.StaticData, _prefabFactory, currentLevelNum);
            IAIManager aiManager = new AIManager(_prefabFactory, _deferrer, _dataProvider);
            aiManager.CreateAI(levelInfo);
		}

        public IActivenessDecider<IBuildable> CreateBuildableButtonActivenessDecider(IDroneManager droneManager)
        {
            return new BuildableAffordableDecider(droneManager);
        }

        public IActivenessDecider<BuildingCategory> CreateCategoryButtonActivenessDecider()
        {
            // For the real game want to enable all building categories :)
            return new BuildingCategoryStaticDecider(shouldBeEnabled: true);
        }

        public IActivenessDecider<IBuilding> CreateBuildingDeleteButtonActivenessDecider(ICruiser playerCruiser)
        {
            return new NormalBuildingDeleteButtonDecider(playerCruiser);
        }
    }
}
