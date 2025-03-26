namespace BattleCruisers.Data
{
    public enum GameMode
    {
        Campaign = 1,
        Tutorial = 2,
        Skirmish = 3,
        PvP_1VS1 = 4,
        CoinBattle = 5,
        SideQuest = 6

        //Voyage = 5
    }

    public static class ApplicationModel
    {
        public static int SelectedLevel
        {
            get => DataProvider.GameModel.SelectedLevel;
            set
            {
                DataProvider.GameModel.SelectedLevel = value;
                DataProvider.SaveGame();
            }
        }

        public static int SelectedPvPLevel
        {
            get => DataProvider.GameModel.SelectedPvPLevel;
            set
            {
                DataProvider.GameModel.SelectedPvPLevel = value;
                DataProvider.SaveGame();
            }
        }

        public static int SelectedSideQuestID
        {
            get => DataProvider.GameModel.SelectedSideQuestID;
            set
            {
                DataProvider.GameModel.SelectedSideQuestID = value;
                DataProvider.SaveGame();
            }
        }

        public static bool ShowPostBattleScreen { get; set; }
#if UNITY_EDITOR
        public static GameMode Mode { get; set; } = GameMode.Campaign;
#else
        public static GameMode Mode { get; set; }
#endif
        public static bool IsTutorial => Mode == GameMode.Tutorial;
        public static bool UserWonSkirmish { get; set; }

        public static void Initialise()
        {
            ShowPostBattleScreen = false;
            Mode = GameMode.Campaign;
            UserWonSkirmish = false;
        }
    }
}
