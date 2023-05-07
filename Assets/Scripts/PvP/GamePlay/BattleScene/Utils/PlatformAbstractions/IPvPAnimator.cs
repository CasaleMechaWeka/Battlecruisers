namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public interface IPvPAnimator
    {
        float Speed { get; set; }
        void Play(string stateName, int layer, float normalizedTime);
    }
}