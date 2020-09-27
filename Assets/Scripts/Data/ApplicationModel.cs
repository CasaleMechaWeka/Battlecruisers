using UnityEngine.Assertions;

namespace BattleCruisers.Data
{
    public class ApplicationModel : IApplicationModel
    {
        // FELIX Remove :)
        public const int DEFAULT_SELECTED_LEVEL = -1;

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
        public bool IsTutorial { get; set; }
        public IDataProvider DataProvider { get; }

        public ApplicationModel(IDataProvider dataProvider)
        {
            Assert.IsNotNull(dataProvider);

            DataProvider = dataProvider;
            ShowPostBattleScreen = false;
            IsTutorial = false;
        }
    }
}
