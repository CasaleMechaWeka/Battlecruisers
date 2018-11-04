namespace BattleCruisers.Data
{
    public interface IApplicationModel
    {
        int SelectedLevel { get; set; }
        bool ShowPostBattleScreen { get; set; }
        bool IsTutorial { get; set; }
        IDataProvider DataProvider { get; }
    }
}