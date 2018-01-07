namespace BattleCruisers.UI.Sound
{
    public enum SoundType
    {
        Deaths, Engines, Explosions, Firing
    }

    public interface ISoundKey
    {
        SoundType Type { get; }
        string Name { get; }
    }
}
