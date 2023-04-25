using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog
{
    public enum PvPFogStrength
    {
        Weak, Strong
    }

    public class PvPFogOfWar : PvPMonoBehaviourWrapper
    {
        public GameObject weakFog, strongFog;

        public void Initialise(PvPFogStrength fogStrength)
        {
            Helper.AssertIsNotNull(weakFog, strongFog);

            IsVisible = false;

            weakFog.SetActive(fogStrength == PvPFogStrength.Weak);
            strongFog.SetActive(fogStrength == PvPFogStrength.Strong);
        }
    }
}