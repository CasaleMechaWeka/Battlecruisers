using UnityEngine;

namespace BattleCruisers.Fetchers
{
    public interface IMaterialFetcher
    {
        Material GetMaterial(string materialName);
    }
}
