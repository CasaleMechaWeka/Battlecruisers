using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BattleCruisers.Utils.Debugging
{
    public class EditorOnlyGameObject : MonoBehaviour
    {
        // Activate or deactivate children in the editor
        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                // Activate or deactivate children based on the parent's active state
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(gameObject.activeSelf);
                }
            }
#endif
        }
    }
}