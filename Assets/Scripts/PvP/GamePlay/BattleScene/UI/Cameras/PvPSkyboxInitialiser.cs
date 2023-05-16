using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras
{
    public class PvPSkyboxInitialiser : MonoBehaviour
    {
        public async Task InitialiseAsync(Skybox skybox, ILevel level)
        {
            PvPHelper.AssertIsNotNull(skybox, level);

            IPvPMaterialFetcher materialFetcher = new PvPMaterialFetcher();
            skybox.material = await materialFetcher.GetMaterialAsync(level.SkyMaterialName);
        }
    }
}