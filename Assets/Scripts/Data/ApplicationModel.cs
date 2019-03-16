using UnityEngine.Assertions;

namespace BattleCruisers.Data
{
    /// <summary>
    /// Static class that allows scenes to communicate with each other.
    /// </summary>
    public class ApplicationModel : IApplicationModel
    {
        public int SelectedLevel { get; set; }
        public bool ShowPostBattleScreen { get; set; }
        public bool IsTutorial { get; set; }
        public IDataProvider DataProvider { get; }

        public ApplicationModel(IDataProvider dataProvider)
        {
            Assert.IsNotNull(dataProvider);

            DataProvider = dataProvider;
            SelectedLevel = -1;
            ShowPostBattleScreen = false;
            IsTutorial = false;
        }
    }
}
