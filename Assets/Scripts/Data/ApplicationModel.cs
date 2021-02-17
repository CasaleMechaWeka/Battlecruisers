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
        // FELIX  Remove :)
        public bool IsTutorial { get; set; }
        public GameMode Mode { get; set; }
        public IDataProvider DataProvider { get; }

        public ApplicationModel(IDataProvider dataProvider)
        {
            Assert.IsNotNull(dataProvider);

            DataProvider = dataProvider;
            ShowPostBattleScreen = false;
            IsTutorial = false;
            Mode = GameMode.Campaign;
        }
    }
}
