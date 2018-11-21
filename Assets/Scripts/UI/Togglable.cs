using UnityEngine;

namespace BattleCruisers.UI
{
    public class Togglable : MonoBehaviour, ITogglable
    {
        public bool Enabled { set { enabled = value; } }
    }
}