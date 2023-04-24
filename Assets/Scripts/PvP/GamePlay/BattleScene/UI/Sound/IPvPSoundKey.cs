
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound
{

    public enum PvPSoundType
    {
        Deaths, Engines, Explosions, Firing, Completed, Events, Music, UI, Shields
    }

    public interface IPvPSoundKey
    {
        PvPSoundType Type { get; }
        string Name { get; }
    }


}

