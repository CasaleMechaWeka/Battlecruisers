using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
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
