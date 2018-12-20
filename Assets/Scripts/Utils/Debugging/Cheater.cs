using BattleCruisers.Cruisers;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Utils.Debugging
{
    // TEMP  Turn this class off for final game :P
    public class Cheater : MonoBehaviour
    {
        void Start()
        {
            if (!Debug.isDebugBuild)
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.W))
            {
                Cruiser[] cruisers = FindObjectsOfType<Cruiser>();
                Cruiser aiCruiser = cruisers.FirstOrDefault(cruiser => cruiser.Faction == Buildables.Faction.Reds);

                if (aiCruiser != null)
                {
                    aiCruiser.TakeDamage(aiCruiser.MaxHealth, null);
                }
            }
        }
    }
}
