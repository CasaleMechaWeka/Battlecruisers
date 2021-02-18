using BattleCruisers.Data.Settings;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public interface IPostBattleState
    {
        bool ShowDifficultySymbol { get; }
        Difficulty Difficulty { get; }
        bool ShowVictoryBackground { get; }
    }
}