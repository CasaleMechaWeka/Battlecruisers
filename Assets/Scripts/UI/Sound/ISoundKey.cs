namespace BattleCruisers.UI.Sound
{
    /// <summary>
    /// Types match the folder names in Resource/Sounds/ !
    /// Hence, do not change :P
    /// </summary>
    public enum SoundType
    {
        Deaths, Engines, Explosions, Firing, Completed, Events, Music, UI, Shields
    }

    public interface ISoundKey
    {
        SoundType Type { get; }
        string Name { get; }
    }
}
