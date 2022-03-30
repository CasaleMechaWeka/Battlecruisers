using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class CanvasGroupButtonChunky : CanvasGroupButton
    {
        protected override ISoundKey ClickSound => SoundKeys.UI.ChunkyClick;
    }
}