namespace BattleCruisers.Data
{
    public enum GameMode
    {
        Campaign = 1,
        Tutorial = 2,
        Skirmish = 3,
        PvP_1VS1 = 4,

        //Voyage = 5
    }

    public interface IApplicationModel
    {

        int SelectedLevel { get; set; }
        bool ShowPostBattleScreen { get; set; }
        bool UserWonSkirmish { get; set; }
        GameMode Mode { get; set; }
        bool IsTutorial { get; }
        IDataProvider DataProvider { get; }

    }
}