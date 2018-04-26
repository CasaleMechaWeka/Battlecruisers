using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.BattleScene.Manager;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes
{
    public class TutorialHelper : IBattleSceneHelper
    {
        private readonly IDataProvider _dataProvider;

        public TutorialHelper(IDataProvider dataProvider)
        {
            Assert.IsNotNull(dataProvider);
            _dataProvider = dataProvider;
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
    }
}
