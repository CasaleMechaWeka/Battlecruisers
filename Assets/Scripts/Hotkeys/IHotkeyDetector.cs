using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys
{
    public interface IHotkeyDetector : IManagedDisposable
    {
        // Navigation
        event EventHandler PlayerCruiser, Overview, EnemyCruiser;

        // Game speed
        event EventHandler PauseSpeed, SlowMotion, NormalSpeed, FastForward, ToggleSpeed;

        // Building categories
        event EventHandler Factories, Defensives, Offensives, Tacticals, Ultras;

        // Factories
        event EventHandler FactoryButton1, FactoryButton2, FactoryButton3, FactoryButton4, FactoryButton5;

        // Defensives
        event EventHandler DefensiveButton1, DefensiveButton2, DefensiveButton3, DefensiveButton4, DefensiveButton5;

        // Offensives
        event EventHandler OffensiveButton1, OffensiveButton2, OffensiveButton3, OffensiveButton4, OffensiveButton5;

        // Tacticals
        event EventHandler TacticalButton1, TacticalButton2, TacticalButton3, TacticalButton4, TacticalButton5;

        // Ultras
        event EventHandler UltraButton1, UltraButton2, UltraButton3, UltraButton4, UltraButton5;

        // Aircraft
        event EventHandler AircraftButton1, AircraftButton2, AircraftButton3, AircraftButton4, AircraftButton5;

        // Ships
        event EventHandler ShipButton1, ShipButton2, ShipButton3, ShipButton4, ShipButton5;
    }
}