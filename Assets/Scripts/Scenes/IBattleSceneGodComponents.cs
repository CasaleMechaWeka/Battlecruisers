using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Scenes
{
    public interface IBattleSceneGodComponents
    {
        IDeferrer Deferrer { get; }
        IVariableDelayDeferrer VariableDelayDeferrer { get; }
        IHighlightFactory HighlightFactory { get; }
        IAudioSource AudioSource { get; }
    }
}