namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public enum PvPBuildSpeed
    {
        InfinitelySlow, // Buildables progress but never complete
        Normal,         // Buildables complete as they should
        VeryFast        // Buildables complete very quickly
    }

    public interface IPvPBuildSpeedController
    {
        PvPBuildSpeed BuildSpeed { set; }
    }
}
