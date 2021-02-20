using BattleCruisers.Data.Models;
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

        public bool ShowPostBattleScreen { get; set; }
        public GameMode Mode { get; set; }
        public bool IsTutorial => Mode == GameMode.Tutorial;
        public IDataProvider DataProvider { get; }
        // FELIX  Remove :D
        public ISkirmishModel Skirmish { get; set; }
        public bool UserWonSkirmish { get; set; }

        public ApplicationModel(IDataProvider dataProvider)
        {
            Assert.IsNotNull(dataProvider);

            DataProvider = dataProvider;
            ShowPostBattleScreen = false;
            Mode = GameMode.Campaign;
            Skirmish = null;
            UserWonSkirmish = false;
        }
    }
}
