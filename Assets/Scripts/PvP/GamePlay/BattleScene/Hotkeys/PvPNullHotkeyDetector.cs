using BattleCruisers.Hotkeys;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys
{
    public class PvPNullHotkeyDetector : IHotkeyDetector
    {
#pragma warning disable 67  // Unused event
        // Navigation
        public event EventHandler PlayerCruiser, Overview, EnemyCruiser;

        // Game speed
        public event EventHandler PauseSpeed, SlowMotion, NormalSpeed, FastForward, ToggleSpeed;

        // Building categories
        public event EventHandler Factories, Defensives, Offensives, Tacticals, Ultras;

        // Factories
        public event EventHandler FactoryButton1, FactoryButton2, FactoryButton3, FactoryButton4, FactoryButton5;

        // Defensives
        public event EventHandler DefensiveButton1, DefensiveButton2, DefensiveButton3, DefensiveButton4, DefensiveButton5;

        // Offensives
        public event EventHandler OffensiveButton1, OffensiveButton2, OffensiveButton3, OffensiveButton4, OffensiveButton5;

        // Tacticals
        public event EventHandler TacticalButton1, TacticalButton2, TacticalButton3, TacticalButton4, TacticalButton5;

        // Ultras
        public event EventHandler UltraButton1, UltraButton2, UltraButton3, UltraButton4, UltraButton5;

        // Aircraft
        public event EventHandler AircraftButton1, AircraftButton2, AircraftButton3, AircraftButton4, AircraftButton5;

        // Ships
        public event EventHandler ShipButton1, ShipButton2, ShipButton3, ShipButton4, ShipButton5;
#pragma warning restore 67  // Unused event

        public void DisposeManagedState() { }
    }
}