using UnityEngine.Assertions;

namespace BattleCruisers.Data
{
    public class ApplicationModel : IApplicationModel
    {
        public int SelectedLevel
        {
            get => DataProvider.GameModel.SelectedLevel;
            set
            {
                DataProvider.GameModel.SelectedLevel = value;
                DataProvider.SaveGame();
            }
        }

        public int SelectedPvPLevel
        {
            get => DataProvider.GameModel.SelectedPvPLevel;
            set
            {
                DataProvider.GameModel.SelectedPvPLevel = value;
                DataProvider.SaveGame();
            }
        }

        public bool ShowPostBattleScreen { get; set; }
        public GameMode Mode { get; set; }
        public bool IsTutorial => Mode == GameMode.Tutorial;
        public IDataProvider DataProvider { get; }
        public bool UserWonSkirmish { get; set; }

        public ApplicationModel(IDataProvider dataProvider)
        {
            Assert.IsNotNull(dataProvider);

            DataProvider = dataProvider;
            ShowPostBattleScreen = false;
            Mode = GameMode.Campaign;
            UserWonSkirmish = false;
        }
    }
}
