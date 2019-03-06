using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Scenes.BattleScene
{
    public interface IBattleSceneGodComponents
    {
        IDeferrer Deferrer { get; }
        IVariableDelayDeferrer VariableDelayDeferrer { get; }
        IAudioSource AudioSource { get; }
        ICamera Camera { get; }
        IMarkerFactory MarkerFactory { get; }
    }
}