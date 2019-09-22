using BattleCruisers.Buildables;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public interface IDroneAudioActivenessDecider
    {
        bool ShouldHaveAudio(Faction droneFaction);
    }
}