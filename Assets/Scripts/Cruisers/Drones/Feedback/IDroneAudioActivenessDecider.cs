using BattleCruisers.Buildables;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public interface IDroneAudioActivenessDecider
    {
        // FELIX  Rename, shorten
        bool ShouldDroneAudioBeActive(Faction droneFaction);
    }
}