using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.Music
{
    // Can create different implementations.  Eg, could have an implementation with 
    // a SwitchTheme() method, which switches between the Kentient and Experimental
    // themes.
    // FELIX  Remove :P
    public interface IMusicProvider
    {
        ISoundKey ScreensSceneKey { get; }
        ISoundKey BattleSceneKey { get; }
        ISoundKey DangerKey { get; }
        ISoundKey VictoryKey { get; }
    }
}