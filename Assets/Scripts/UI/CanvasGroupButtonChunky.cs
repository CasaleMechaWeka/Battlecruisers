using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI
{
    public class CanvasGroupButtonChunky : CanvasGroupButton
    {
        protected override SoundKey ClickSound => SoundKeys.UI.ChunkyClick;
    }
}