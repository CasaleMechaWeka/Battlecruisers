using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
{
    public interface IMaterialFetcher
    {
        Material GetMaterial(string materialName);
    }
}
