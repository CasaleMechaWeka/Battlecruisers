namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface IBackgroundStatsProvider
    {
        IBackgroundImageStats GetStats(int levelNum);
    }
}