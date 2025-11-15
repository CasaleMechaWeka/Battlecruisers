using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class Bodykit : Prefab
    {
        public Sprite bodykitImage;
        public Sprite BodykitImage => bodykitImage;

        public HullType cruiserType;
        public HullType CruiserType => cruiserType;
    }

    public enum HullType
    {
        None        = -1, // no bodykit
        AlphaSpace  = 31,
        Arkdeso     = 32,
        Axiom       = 28,
        BasicRig    = 18,
        BlackRig    = 11,
        Bullshark   = 0,
        Cricket     = 19,
        Eagle       = 1,
        EndlessWall = 30,
        Essex       = 27,
        Flea        = 16,
        FortNova    = 20,
        Goatherd    = 13,
        Hammerhead  = 2,
        Longbow     = 3,
        Megalith    = 17,
        Megalodon   = 4,
        Microlodon  = 12,
        Middlodon   = 26,
        October     = 29,
        Orac        = 25,
        Pistol      = 15,
        Raptor      = 5,
        Rickshaw    = 10,
        Rockjaw     = 6,
        Salvage     = 24,
        Shepherd    = 14,
        TasDevil    = 8,
        TekGnosis   = 23,
        Trident     = 7,
        Yeti        = 9,
        Yucalux     = 22,
        Zumwalt     = 21
    }
}
