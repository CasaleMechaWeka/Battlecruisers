using UnityEngine;

namespace BattleCruisers.Utils.BattleScene
{
    public class WaterImpactHandler : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(collision.gameObject);

            // FELIX  Make splash
        }
    }
}