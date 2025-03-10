using BattleCruisers.Data.Settings;
using System;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public interface IDifficultyDropdown
    {
        Difficulty Difficulty { get; }

        event EventHandler DifficultyChanged;
    }
}