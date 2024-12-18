using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public class SkyboxInitialiser : MonoBehaviour
    {
        public async Task InitialiseAsync(Skybox skybox, string skyMaterialName)
        {
            Helper.AssertIsNotNull(skybox, skyMaterialName);

            IMaterialFetcher materialFetcher = new MaterialFetcher();
            skybox.material = await materialFetcher.GetMaterialAsync(skyMaterialName);
        }
    }
}