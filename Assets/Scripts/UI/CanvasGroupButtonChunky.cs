using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI
{
    public class CanvasGroupButtonChunky : CanvasGroupButton
    {
        protected override ISoundKey ClickSound => SoundKeys.UI.ChunkyClick;
    }
}