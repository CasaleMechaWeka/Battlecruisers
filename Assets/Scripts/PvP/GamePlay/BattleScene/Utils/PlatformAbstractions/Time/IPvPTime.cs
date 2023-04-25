

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time
{
    public interface IPvPTime : IPvPDeltaTimeProvider
    {
        float TimeScale { get; set; }
        float TimeSinceGameStartInS { get; }
        float UnscaledTimeSinceGameStartInS { get; }
        float UnscaledDeltaTime { get; }

        IPvPTimeSinceGameStartProvider TimeSinceGameStartProvider { get; }
        IPvPTimeSinceGameStartProvider RealTimeSinceGameStartProvider { get; }
    }
}

