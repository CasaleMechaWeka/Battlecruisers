namespace BattleCruisers.Utils.Debugging
{
    public interface ICheater
    {
        void AddBuilders();
        void Win();
        void Lose();
        void ShowNuke();
        void TogglePause();
        void ToggleUI();
    }
}