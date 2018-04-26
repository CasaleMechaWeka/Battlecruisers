using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Scenes
{
    public class TutorialHelper : IBattleSceneHelper
    {
        private readonly IDataProvider _dataProvider;
        private readonly IPrefabFactory _prefabFactory;

        public TutorialHelper(IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
        }

        public IUIManager CreateUIManager(IManagerArgs args)
        {
            IUIManagerPermissions permissions = new UIManagerPermissions()
            {
                CanDismissItemDetails = false,
                CanShowItemDetails = false
            };
            return new LimitableUIManager(args, permissions);
        }

        public ILoadout GetPlayerLoadout()
        {
            return _dataProvider.StaticData.InitialGameModel.PlayerLoadout;
        }
		
        public void CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum)
		{
            // The tutorial has no AI :)
		}

        public IBuildableButtonActivenessDecider<IBuildable> CreateButtonActivenessDecider(IDroneManager droneManager)
        {
            return new TutorialDecider(_prefabFactory);
        }
    }
}
