using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class CartoonExplosion : MonoBehaviour
    {
        public void Initialise(bool showTrails)
        {
            GameObject trails = transform.FindNamedComponent<GameObject>("FireworkExplosion");
            trails.SetActive(showTrails);
        }
    }
}