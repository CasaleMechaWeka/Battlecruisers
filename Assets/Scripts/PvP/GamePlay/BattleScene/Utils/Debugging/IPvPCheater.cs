namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Debugging
{
    public interface IPvPCheater
    {
        void AddBuilders();
        void Win();
        void Lose();
        void ShowNuke();
        void TogglePause();
        void ToggleUI();
        void SetSpeedNormal();
        void SetSpeedFast();
        void SetSpeedVeryFast();
        void ToggleCursor();
    }
}