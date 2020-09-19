using BattleCruisers.Data.Static;
using UnityEngine.Assertions;

namespace BattleCruisers.Data
{
    public class ApplicationModel : IApplicationModel
    {
        private int _selectedLevel;
        public int SelectedLevel 
        {
            get => _selectedLevel;
            set
            {
                Assert.IsTrue(value > 0);
                Assert.IsTrue(value <= StaticData.NUM_OF_LEVELS);

                _selectedLevel = value;
            }
        }

        public bool ShowPostBattleScreen { get; set; }
        public bool IsTutorial { get; set; }
        public IDataProvider DataProvider { get; }

        public ApplicationModel(IDataProvider dataProvider)
        {
            Assert.IsNotNull(dataProvider);

            DataProvider = dataProvider;
            _selectedLevel = -1;
            ShowPostBattleScreen = false;
            IsTutorial = false;
        }
    }
}
