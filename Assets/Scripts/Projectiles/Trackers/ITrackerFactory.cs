namespace BattleCruisers.Projectiles.Trackers
{
    public interface ITrackerFactory
    {
        Tracker CreateTracker(ITrackable trackable);
    }
}