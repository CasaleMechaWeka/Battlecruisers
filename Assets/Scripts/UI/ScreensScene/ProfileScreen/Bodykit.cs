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
        Bullshark,
        Eagle,
        Hammerhead,
        Longbow,
        Megalodon,
        Raptor,
        Rockjaw,
        Trident,
        TasDevil,
        Yeti,
        Rickshaw,
        BlackRig,
        Microlodon,
        Goatherd,
        Shepherd,
        Pistol,
        Flea,
        Megalith,
        BasicRig,
        Cricket,
        FortNova,
        Zumwalt,
        Yucalux,
        TekGnosis,
        Salvage,
        Orac,
        Middlodon,
        Essex,
        Axiom,
        October,
        EndlessWall,
        AlphaSpace,
        Arkdeso,
        None   //  no bodykit
    }
}
