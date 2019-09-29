using BattleCruisers.Buildables;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    // FELIX  Remove :P
    public interface IDroneAudioActivenessDecider
    {
        bool ShouldHaveAudio(Faction droneFaction);
    }
}