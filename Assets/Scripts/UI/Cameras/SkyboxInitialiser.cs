using BattleCruisers.Data;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public class SkyboxInitialiser : MonoBehaviour
    {
        public void Initialise(Skybox skybox, ILevel level)
        {
            Helper.AssertIsNotNull(skybox, level);

            IMaterialFetcher materialFetcher = new MaterialFetcher();
            skybox.material = materialFetcher.GetMaterial(level.SkyMaterialName);
        }
    }
}