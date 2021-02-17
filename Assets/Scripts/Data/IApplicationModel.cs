namespace BattleCruisers.Data
{
    public enum GameMode
    {
        Campaign = 1,
        Tutorial = 2,
        Skirmish = 3
    }

    public interface IApplicationModel
    {
        int SelectedLevel { get; set; }
        bool ShowPostBattleScreen { get; set; }
        // FELIX  Remove :)
        bool IsTutorial { get; set; }
        GameMode Mode { get; set; }
        IDataProvider DataProvider { get; }
    }
}