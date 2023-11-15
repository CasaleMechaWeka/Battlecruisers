namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public enum PvPSmokeStrength
    {
        None, Weak, Normal, Strong
    }

    public interface IPvPSmoke
    {
        PvPSmokeStrength SmokeStrength { get; set; }
    }
}