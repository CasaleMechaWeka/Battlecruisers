namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Movement
{
    public interface IPvPMovementEffect
    {
        void Show();
        void StartEffects();
        void StopEffects();
        void ResetAndHide();
    }
}